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
    public class VisitedRoutesController : ControllerBase
    {
        private readonly DBWorker _worker;

        public VisitedRoutesController() => _worker = new DBWorker();

        /// <summary>
        /// Получить список пройденных маршрутов из личного кабинета
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetVisitedRoutes")]
        public IEnumerable<Route> GetRoute(int userId) => _worker.GetVisitedRoutes(userId);

        /// <summary>
        /// Добавить в пройденное
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="routeId"></param>
        /// <param name="visit"></param>
        /// <returns></returns>
        [HttpPut(Name = "PutVisitedRoute")]
        public string PutVisitedRoute(VisitedRoutePut route)
        {
            _worker.PutVisitedRoute(route.UserId, route.RouteId, route.IsVisited);
            return "Success";
        }
    }
}
