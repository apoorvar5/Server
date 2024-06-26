﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using Server.DTO;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController(PlayerSourceContext context) : ControllerBase
    {

        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            return await context.Clubs.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetPlayerCount")]
        public async Task<ActionResult<IEnumerable<PlayerCount>>> GetPlayerCount()
        {
            IQueryable<PlayerCount> x = context.Clubs.Select(c =>
            new PlayerCount 
            {
            ClubName=c.ClubName,
            ClubId=c.ClubId,
            CountPlayer=c.PlayerClub.Select(avc => avc.PlayerId).Distinct().Count(),
            }
            );
            return await x.ToListAsync();
        }

        //
        // GET: api/Clubs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> GetClub(int id)
        {
            var club = await context.Clubs.FindAsync(id);

            if (club == null)
            {
                return NotFound();
            }

            return club;
        }

        /* [HttpGet("ClubPlayers/{id}")]
         public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByClub(int id)
         {
             return await context.Players.Where(c => c.ClubId == id).ToListAsync();
         }*/

        [Authorize]
        [HttpGet("ClubPlayers/{id}")]
        public async Task<ActionResult<List<object>>> GetPlayersByClub(int id)
        {
            var results = await context.PlayerClub
                .Where(avc => avc.ClubId == id)
                .Select(avc => new {
                    ClubId = avc.ClubId,
                    Player = new
                    {
                        PlayerId = avc.Player.PlayerId,
                        PlayerName = avc.Player.PlayerName,
                        PlayerPos = avc.Player.PlayerPos,
                        PlayerNationality=avc.Player.PlayerNationality
                    },
                })
                .ToListAsync();
            return Ok(results);
        }

        // PUT: api/Clubs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(int id, Club club)
        {
            if (id != club.ClubId)
            {
                return BadRequest();
            }

            context.Entry(club).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Clubs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        public async Task<ActionResult<Club>> PostClub([FromForm] NewClubDto newClubDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = $"INSERT INTO Club (ClubName, ClubLeague, ClubCountry) VALUES " + $"('{newClubDto.ClubName}', '{newClubDto.ClubLeague}','{newClubDto.ClubCountry}')";
            await context.Database.ExecuteSqlRawAsync(query);
            return Ok();
        }

        [HttpPost("{id}/AddPlayer")]
        public async Task<ActionResult> AddPlayer(int id, [FromForm] NewPlayerDto newPlayerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = new Player
            {
                PlayerName = newPlayerDto.PlayerName,
                PlayerPos = newPlayerDto.PlayerPos,
                PlayerNationality = newPlayerDto.PlayerNationality,

            };
            context.Players.Add(player);
            await context.SaveChangesAsync();

            var playerId = player.PlayerId;

            var playerClub = new PlayerClub
            {
                ClubId = id,
                PlayerId = playerId,
            };
            context.PlayerClub.Add(playerClub);
            await context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Clubs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club= await context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            context.Clubs.Remove(club);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClubExists(int id)
        {
            return context.Clubs.Any(e => e.ClubId == id);
        }
    }
}
