using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Rest_Server.DTOs;
using Rest_Server.Services;

namespace Rest_Server.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly DBWorker _worker;

        public UserAuthController() => _worker = new DBWorker();

        /// <summary>
        /// Проверка, занят ли логин
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetRegistration")]
        public bool Get(string login) => _worker.CheckLogin(login);

        /// <summary>
        /// Получить страничку пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}")]
        public UserPage Get(int userId) => _worker.GetUser(userId);

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        [HttpPost(Name ="PostUser")] 
        public void UserRegistration(UserRegistration user) => _worker.CreateUser(user.Login, user.Password);
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="mail"></param>
        /// <param name="phone"></param>
        [HttpPut(Name = "UpdateUser")]
        public void UpdateUser(UpdateUser user) => _worker.UpdateUser(user.UserId, user.Nickname, user.Mail, user.Phone);
    }
}
