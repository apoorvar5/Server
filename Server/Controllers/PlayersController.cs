using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using Server.DTO;

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
    }
}
