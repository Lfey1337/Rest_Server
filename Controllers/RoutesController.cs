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
    public class RoutesController : ControllerBase
    {
        private readonly DBWorker _worker;

        public RoutesController() => _worker = new DBWorker();
        /// <summary>
        /// Получить список маршрутов
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetRoutes")]
        public IEnumerable<Route> GetRoutes(int userId)
        {
            if (ModelState.IsValid)
            {
                return _worker.GetRoutes(userId);
            }
            else
            {
                NotFound();
                return GetRoutes(userId);
            }
        }
        /// <summary>
        /// Получить конкретный маршрут
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        [HttpGet("{routeId:int}")]
        public IEnumerable<RoutePage> GetRoute(int routeId, int userId)
        {
            return _worker.GetRoute(routeId, userId);
        }
        /// <summary>
        /// Найти маршуты по ключевым словам
        /// </summary>
        /// <returns></returns>
        [HttpGet("@{query}")]
        public IEnumerable<Route> GetSearch(string query)
        {
            return _worker.GetSearchRoutes(query);
        }
    }
}
