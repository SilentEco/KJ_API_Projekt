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

namespace KJ_API_Projekt.Controllers
{
    namespace v1
    {
        [ApiController]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/[controller]")]
        public class GeoMessagesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public GeoMessagesController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/GeoMessages
            [HttpGet("[action]")]

            public async Task<ActionResult<IEnumerable<GeoMessages>>> GetgeoMessages()
            {
                List<GeoMessagesV2> v2list = await _context.geoMessagesV2.Include(a => a.Message).ToListAsync();
                List<GeoMessages> v1list = new List<GeoMessages>();
                foreach (var item in v2list)
                {
                    GeoMessages geoMessages = new GeoMessages { Message = item.Message.Body, latitude = item.latitude, longitude = item.longitude};
                    v1list.Add(geoMessages);
                }
                return v1list;
            }

            // GET: api/GeoMessages/5
            [HttpGet("[action]/{id}")]

            public async Task<ActionResult<GeoMessages>> GetGeoMessages(int id)
            {
                var geoMessages = await _context.geoMessagesV2.Include(a => a.Message).FirstOrDefaultAsync(b => b.Id == id);

                if (geoMessages == null)
                {
                    return NotFound();
                }
                var v1modell = new GeoMessages
                {
                    Message = geoMessages.Message.Body,
                    longitude = geoMessages.longitude,
                    latitude = geoMessages.latitude
                };
                return v1modell;
            }

            // POST: api/GeoMessages
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost("[action]")]
            [Authorize]
            public async Task<ActionResult<GeoMessages>> PostGeoMessages(GeoMessages geoMessages)
            {
                var v2modell = new GeoMessagesV2 { 
                    Message = new Message { 
                    Body = geoMessages.Message },
                    longitude = geoMessages.longitude, 
                    latitude = geoMessages.latitude };
                _context.geoMessagesV2.Add(v2modell);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessages", new { id = geoMessages.Id }, geoMessages);
            }

        }
    }
    
    namespace v2
    {
        [ApiController]
        [ApiVersion("2.0")]
        [Route("api/v{version:apiVersion}/[controller]")]
        public class GeoMessagesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public GeoMessagesController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet("[action]/{id}")]

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
               // return await _context.geoMessagesV2.Include(a => a.Message).ToListAsync();
            }

            [HttpGet("[action]")]

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


            [HttpPost("[action]")]
            
            public async Task<ActionResult<GeoMessagesV2>> PostGeoMessages(GeoMessagesV2 geoMessages)
            {
                
                _context.geoMessagesV2.Add(geoMessages);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessages", new { id = geoMessages.Id }, geoMessages);
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


            #endregion



        }

    }
}
