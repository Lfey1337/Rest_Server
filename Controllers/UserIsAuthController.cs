using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Rest_Server.DTOs;
using Rest_Server.Services;

namespace Rest_Server.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserIsAuthController : ControllerBase
    {
        private readonly DBWorker _worker;

        public UserIsAuthController() => _worker = new DBWorker();
        /// <summary>
        /// Получить Id, Nickname и img + extension юзера
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetUserId")]
        public UserIsAuth Get(string login) => _worker.GetUser(login);
    }
}
