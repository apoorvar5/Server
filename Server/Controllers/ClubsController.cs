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
    public class ClubsController(PlayerSourceContext context) : ControllerBase
    {

        // GET: api/Clubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClubs()
        {
            return await context.Clubs.ToListAsync();
        }
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
    }
}
