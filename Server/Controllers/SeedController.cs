using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using Microsoft.AspNetCore.Identity;

namespace animeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(PlayerSourceContext db, IHostEnvironment environment, UserManager<ClubPlayerUser> userManager) : ControllerBase
    {

        [HttpPost("User")]
        public async Task<ActionResult> SeedUser()
        {
            (string name, string email) = ("user1", "comp584@csun.edu");
            ClubPlayerUser user = new()
            {
                UserName = name,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            if (await userManager.FindByNameAsync(name) is not null)
            {
                user.UserName = "user2";
            }
            _ = await userManager.CreateAsync(user, "P@ssw0rd!")
                ?? throw new InvalidOperationException();
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}