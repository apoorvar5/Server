using CountryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<ClubPlayerUser> userManager, JwtHandler jwtHandler)
    {
        [HttpPost]
        public void Login() {
        
        }
    }
}
