using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rest_Server.DTOs;
using Rest_Server.Services;

namespace Rest_Server.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RouteCommentsController : ControllerBase
    {
        private readonly DBWorker _worker;

        public RouteCommentsController() => _worker = new DBWorker();

        /// <summary>
        /// Получить комментарии по маршруту
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetRouteComments")]
        public IEnumerable<Comment> Get(int routeId) => _worker.GetRouteComments(routeId);

        /// <summary>
        /// Вставить комментарий по маршруту
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="userId"></param>
        /// <param name="comDesc"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostRouteComment")]
        public string Post(RouteCommentPost comment)
        {
            _worker.InsertRouteComment(comment.RouteId, comment.UserId, comment.ComDescription, comment.Rate);
            return "Success";
        }
        /// <summary>
        /// Реализовано, но не добавлено
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteRouteComment")]
        public string Delete(int routeId, int userId, int commentId)
        {
            _worker.DeleteRouteComment(routeId, userId, commentId);
            return "Success";
        }
        /// <summary>
        /// Реализовано, но не добавлено
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <param name="comDescription"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        [HttpPut(Name = "PutRouteComment")]
        public string Put(int routeId, int userId, int commentId, string comDescription, double rate)
        {
            _worker.UpdateRouteComment(routeId, userId, commentId, comDescription, rate);
            return "Success";
        }
    }
}
