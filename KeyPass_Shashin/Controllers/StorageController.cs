using KeyPass_Shashin.Classes;
using KeyPass_Shashin.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Shashin.Controllers
{
    [Route("/storage")]
    public class StorageController : Controller
    {
        private DatabaseManager databaseManager;
        public StorageController()
        {
            this.databaseManager = new DatabaseManager();
        }
        [Route("get")]
        [HttpGet]
        public ActionResult Get([FromHeader] string token)
        {
            try
            {
                int? UserId = JwtToken.GetUserIdFromToken(token);
                if (UserId == null)
                {
                    return StatusCode(401);
                }
                List<StorageDto> Storages = databaseManager.Storages
                    .Where(x => x.User.Id == UserId)
                    .Select(s => new StorageDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Url = s.Url,
                        Login = s.Login,
                        Password = s.Password,
                    })
                    .ToList();
                return Ok(Storages);
            }
            catch (Exception e)
            {
                return StatusCode(501, e.Message);
            }
        }
        [Route("add")]
        [HttpPost]
        public ActionResult Add([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                if (IdUser == null)
                    return StatusCode(401);
                storage.User = databaseManager.Users
                    .Where(x => x.Id == IdUser)
                    .First();

                databaseManager.Add(storage);
                databaseManager.SaveChanges();

                storage.User = null;
                return StatusCode(200, storage);
            }
            catch (Exception e)
            {
                return StatusCode(501, e.Message);
            }
        }
        [Route("update")]
        [HttpPut]
        public ActionResult Update([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                Storage? uStorage = databaseManager.Storages
                    .Where(x => x.Id == storage.Id)
                    .FirstOrDefault();
                if (IdUser == null)
                    return StatusCode(401);
                if (uStorage == null)
                    return StatusCode(404);
                uStorage.Name = storage.Name;
                uStorage.Url = storage.Url;
                uStorage.Login = storage.Login;
                uStorage.Password = storage.Password;
                databaseManager.SaveChanges();
                storage.User = null;
                return StatusCode(200, storage);
            }
            catch (Exception e)
            {
                return StatusCode(501, e.Message);
            }
        }
        [Route("delete")]
        [HttpDelete]
        public ActionResult Delete([FromHeader] string token, [FromForm] int id)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);
                Storage? storage = databaseManager.Storages
                    .Where(x => x.Id == id && x.User.Id == IdUser)
                    .FirstOrDefault();
                if (IdUser == null)
                    return StatusCode(401);
                if (storage == null)
                    return StatusCode(404);
                databaseManager.Storages.Remove(storage);
                databaseManager.SaveChanges();
                return StatusCode(200, storage);
            }
            catch (Exception e)
            {
                return StatusCode(501, e.Message);
            }
        }
    }
}
