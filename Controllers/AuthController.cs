using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Rest_Server.DTOs;
using Rest_Server.Services;

namespace Rest_Server.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DBWorker _worker;

        public AuthController() => _worker = new DBWorker();
        /// <summary>
        /// Валидация логина и пароля
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetAuth")]
        public bool Get(string login, string password) => _worker.GetAuth(login, password);
    }
}
