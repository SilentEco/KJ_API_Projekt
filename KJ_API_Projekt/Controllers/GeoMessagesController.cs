using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KJ_API_Projekt.data;
using KJ_API_Projekt.model;
using Microsoft.AspNetCore.Authorization;
using KJ_API_Projekt.ApiKey;

namespace KJ_API_Projekt.Controllers
{
    namespace v1
    {
        [ApiController]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/")]
        public class GeoMessagesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public GeoMessagesController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet("geo-comments")]
            public async Task<ActionResult<IEnumerable<v1GetDTO>>> GetgeoMessages()
            {
                List<GeoMessagesV2> v2list = await _context.geoMessagesV2.Include(a => a.Message).ToListAsync();
                List<v1GetDTO> v1list = new List<v1GetDTO>();
                foreach (var item in v2list)
                {
                    v1GetDTO geoMessages = new v1GetDTO { Message = item.Message.Body, latitude = item.latitude, longitude = item.longitude};
                    v1list.Add(geoMessages);
                }
                return v1list;
            }

            [HttpGet("geo-comments/{id}")]
            public async Task<ActionResult<v1GetDTO>> GetGeoMessages(int id)
            {
                var geoMessages = await _context.geoMessagesV2.Include(a => a.Message).FirstOrDefaultAsync(b => b.Id == id);

                if (geoMessages == null)
                {
                    return NotFound();
                }
                var v1modell = new v1GetDTO
                {
                    Message = geoMessages.Message.Body,
                    longitude = geoMessages.longitude,
                    latitude = geoMessages.latitude
                };
                return v1modell;
            }

            [HttpPost("geo-comments")]
            [Authorize]
            public async Task<ActionResult<GeoMessagesV2>> PostGeoMessages(v1GetDTO geoMessages)
            {
                var v2modell = new GeoMessagesV2 { 
                    Message = new Message { 
                    Body = geoMessages.Message },
                    longitude = geoMessages.longitude, 
                    latitude = geoMessages.latitude };
                _context.geoMessagesV2.Add(v2modell);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessages", new { id = v2modell.Id }, v2modell);
            }
            #region DTO classer

            public class v1GetDTO
            {
                public string Message { get; set; }
                public double longitude { get; set; }
                public double latitude { get; set; }
            }

            public class v2PostDTO
            {
                public v2MessagePostDTO Message { get; set; }
                public double longitude { get; set; }
                public double latitude { get; set; }
            }

            public class v2MessagePostDTO
            {
                public string Title { get; set; }
                public string Body { get; set; }

            }
            #endregion
        }
    }
    
    namespace v2
    {
        [ApiController]
        [ApiVersion("2.0")]
        [Route("api/v{version:apiVersion}/")]
        public class GeoMessagesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public GeoMessagesController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet("geo-comments/{id}")]

            public async Task<ActionResult<v2GetDTO>> GetgeoMessages(int id)
            {
                var geoDB = await _context.geoMessagesV2.Include(a => a.Message).FirstOrDefaultAsync(o => o.Id == id);

                v2GetDTO geoDTO = new v2GetDTO()
                {
                    latitude = geoDB.latitude,
                    longitude = geoDB.longitude,
                    Message = new v2MessageDTO()
                    {
                        Author = geoDB.Message.Author,
                        Body = geoDB.Message.Body,
                        Title = geoDB.Message.Title

                    }
                };

                return geoDTO; 
            }
            /// <summary>
            /// Visa en  lista av geo comments inom ett visst område
            /// </summary>
            /// <param name="minLon">Minsta Longituden</param>
            /// <param name="maxLon">Högsta Longituden</param>
            /// <param name="minLat">Minsta Latituden</param>
            /// <param name="maxLat">Högsta Latituden</param>
            /// <returns>Returnerar en lista utefter dina valda inputs</returns>
            [HttpGet("geo-comments")]

            public async Task<ActionResult<IEnumerable<v2GetDTO>>> GetgeoMessages(double minLon, double maxLon, double minLat, double maxLat)
            {
                
                var geoDB = await _context.geoMessagesV2.Include(a => a.Message).Where(
                    o => (o.longitude <= maxLon && o.longitude >= minLon) && (o.latitude <= maxLat && o.latitude >= minLat))
                    .ToListAsync();

                List<v2GetDTO> listGeoDTO = new List<v2GetDTO>();
                foreach(var item in geoDB)
                {
                    v2GetDTO geoDTO = new v2GetDTO()
                    {
                        latitude = item.latitude,
                        longitude = item.longitude,
                        Message = new v2MessageDTO()
                        {
                            Author = item.Message.Author,
                            Body = item.Message.Body,
                            Title = item.Message.Title

                        }
                    };

                    listGeoDTO.Add(geoDTO);
                }
                return listGeoDTO;
            }
            [HttpPost("geo-comments")]
            [Authorize]
            
            public async Task<ActionResult<GeoMessagesV2>> PostGeoMessages(v2PostDTO geoMessagesDTO)
            {
                //Försök hämta hem User via api nycklen
                //Här hittar hämtar vi inskriven api nyckel
                string token = Request.Headers[ApiKeyConstants.HttpHeaderField];
                if (token == null)
                    token = Request.Query[ApiKeyConstants.HttpQueryParamKey];
                //Matcha api nyckel med en userID
                var userApiDB = await _context.ApiTokens.FirstOrDefaultAsync(o => o.Value == token);
                var userID = userApiDB.User;

                GeoMessagesV2 geoMessagesV2 = new GeoMessagesV2()
                {
                    Message = new Message()
                    {
                        Author = userApiDB.User.FirstName + " " + userApiDB.User.LastName
                                                ,
                        Body = geoMessagesDTO.Message.Body,
                        Title = geoMessagesDTO.Message.Title
                    },
                    latitude = geoMessagesDTO.latitude,
                    longitude = geoMessagesDTO.longitude

                };
                _context.geoMessagesV2.Add(geoMessagesV2);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessages", new { id = geoMessagesV2.Id }, geoMessagesV2);
            }

            #region DTO classer

            public class v2MessageDTO
            {
                public string Title { get; set; }
                public string Body { get; set; }
                public string Author { get; set; }
            }
            public class v2GetDTO
            {
                

                public v2MessageDTO Message { get; set; }

                public double longitude { get; set; }

                public double latitude { get; set; }


            }

            public class v2PostDTO
            {


                public v2MessagePostDTO Message { get; set; }

                public double longitude { get; set; }

                public double latitude { get; set; }


            }

            public class v2MessagePostDTO
            {
                public string Title { get; set; }
                public string Body { get; set; }
               
            }
            #endregion
        }
    }
}
