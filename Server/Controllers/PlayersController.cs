using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using Server.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController(PlayerSourceContext context) : ControllerBase
    {

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await context.Players.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetClubCount")]
        public async Task<ActionResult<IEnumerable<ClubCount>>> GetClubCount()
        {
            IQueryable<ClubCount> x = context.Players.Select(c =>
            new ClubCount
            {
                PlayerName = c.PlayerName,
                PlayerId = c.PlayerId,
                CountClub = c.PlayerClub.Select(avc => avc.ClubId).Distinct().Count(),
            }
            );
            return await x.ToListAsync();
        }

        [Authorize]
        [HttpGet("PlayerClubs/{id}")]
        public async Task<ActionResult<List<object>>> GetClubsByPlayer(int id)
        {
            var results = await context.PlayerClub
                .Where(avc => avc.PlayerId == id)
                .Select(avc => new {
                    PlayerId = avc.PlayerId,
                    Club = new
                    {
                        ClubId = avc.Club.ClubId,
                        ClubName = avc.Club.ClubName,
                        ClubLeague = avc.Club.ClubLeague,
                        ClubCountry = avc.Club.ClubCountry
                    },
                })
                .ToListAsync();
            return Ok(results);
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            context.Entry(player).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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


        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer([FromForm] NewPlayerDto newPlayerDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = $"INSERT INTO Player (PlayerName, PlayerPos, PlayerNationality) VALUES " + $"('{newPlayerDto.PlayerName}', '{newPlayerDto.PlayerPos}','{newPlayerDto.PlayerNationality}')";
            await context.Database.ExecuteSqlRawAsync(query);
            return Ok();
        }

        [HttpPost("{id}/AddClub")]
        public async Task<ActionResult> AddClub(int id, [FromForm] NewClubDto newClubDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var club = new Club
            {
                ClubName = newClubDto.ClubName,
                ClubLeague = newClubDto.ClubLeague,
                ClubCountry = newClubDto.ClubCountry,

            };
            context.Clubs.Add(club);
            await context.SaveChangesAsync();

            var clubId = club.ClubId;

            var playerClub = new PlayerClub
            {
                ClubId = clubId,
                PlayerId = id,
            };
            context.PlayerClub.Add(playerClub);
            await context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            context.Players.Remove(player);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(int id)
        {
            return context.Players.Any(e => e.PlayerId == id);
        }
    }
}
