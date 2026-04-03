using KeyPass_Shashin.Classes;
using KeyPass_Shashin.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Shashin.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private DatabaseManager databaseManager;
        public UserController()
        {
            this.databaseManager = this.databaseManager = new DatabaseManager();
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login([FromForm] string login, [FromForm] string password)
        {
            try
            {
                User? AuthUser = databaseManager.Users
                    .Where(x => x.Login == login && x.Password == password)
                    .FirstOrDefault();
                if (AuthUser == null)
                {
                    return StatusCode(401);
                } else
                {
                    string Token = JwtToken.Generate(AuthUser);
                    AuthUser.LastAuth = DateTime.Now;
                    databaseManager.SaveChanges();
                    return Ok(new { token = Token });
                }
            } catch (Exception e)
            {
                return StatusCode(501, e.Message);
            }
        } 
    } 
}
