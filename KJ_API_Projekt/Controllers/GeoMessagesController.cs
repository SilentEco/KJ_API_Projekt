﻿using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class GeoMessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GeoMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GeoMessages
        [HttpGet("/api/v1/geo-comments")]
        
        public async Task<ActionResult<IEnumerable<GeoMessages>>> GetgeoMessages()
        {
            return await _context.geoMessages.ToListAsync();
        }

        // GET: api/GeoMessages/5
        [HttpGet("/api/v1/geo-comments/{id}")]
        
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
        [HttpPost("/api/v1/geo-comments")]
        [Authorize]
        public async Task<ActionResult<GeoMessages>> PostGeoMessages(GeoMessages geoMessages)
        {
            _context.geoMessages.Add(geoMessages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessages", new { id = geoMessages.Id }, geoMessages);
        }
 
    }
}
