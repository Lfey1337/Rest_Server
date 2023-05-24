using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Rest_Server.DTOs;
using Rest_Server.Services;
using Route = Rest_Server.DTOs.Route;

namespace Rest_Server.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LikedRoutesController : ControllerBase
    {
        private readonly DBWorker _worker;

        public LikedRoutesController() => _worker = new DBWorker();

        /// <summary>
        /// Получить все понравившиеся маршруты из личного кабинета
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetLikedRoutes")]
        public IEnumerable<Route> GetRoute(int userId) => _worker.GetLikedRoutes(userId);

        /// <summary>
        /// Добавить в понравившееся
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="routeId"></param>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpPut(Name = "PutLikedRoute")]
        public string PutLikedRoute(LikededRoutePut route)
        {
            _worker.PutLikedRoute(route.UserId, route.RouteId, route.IsLiked);
            return "Success";
        }
    }
}
