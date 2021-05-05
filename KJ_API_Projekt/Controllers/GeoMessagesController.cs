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

            [HttpGet("[action]")]

            public async Task<ActionResult<IEnumerable<GeoMessages>>> GetgeoMessages()
            {
                return await _context.geoMessages.ToListAsync();
            }
        }


    }


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
                return await _context.geoMessages.ToListAsync();
            }

            // GET: api/GeoMessages/5
            [HttpGet("[action]/{id}")]

            public async Task<ActionResult<GeoMessages>> GetGeoMessages(int id)
            {
                var geoMessages = await _context.geoMessages.FindAsync(id);

                if (geoMessages == null)
                {
                    return NotFound();
                }

                return geoMessages;
            }

            // POST: api/GeoMessages
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost("[action]")]
            [Authorize]
            public async Task<ActionResult<GeoMessages>> PostGeoMessages(GeoMessages geoMessages)
            {
                _context.geoMessages.Add(geoMessages);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessages", new { id = geoMessages.Id }, geoMessages);
            }

        }
    }
    
}
