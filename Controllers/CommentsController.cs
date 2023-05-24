using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Rest_Server.DTOs;
using Rest_Server.Services;

namespace Rest_Server.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {   
        private readonly DBWorker _worker;

        public CommentsController() => _worker = new DBWorker();

        /// <summary>
        /// Получить все комментарии о маршрутах (из раздела "Главная")
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetComments")]
        public IEnumerable<Comment> Get()
        {
            if (ModelState.IsValid)
            {
                return _worker.GetComments();
            }
            else
            {
                NotFound();
                return Get();
            }
        }
        /// <summary>
        /// Вставить коммент
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostComment")]
        public string Post(CommentPost comment)
        {
            if ((comment.UserId.GetType() == typeof(int) && comment.UserId != 0)
                && !String.IsNullOrEmpty(comment.ComDescription)
                && (comment.Rate.GetType() == typeof(double)))
            {
                _worker.InsertComment(comment.UserId, comment.ComDescription, comment.Rate);
                return "Success";
            }
            else
            {
                NotFound();
                return "Err: wrong type 'comment.' or he is null ";
            }
        }
        /// <summary>
        /// Реализовано, но не используется
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteComment")]
        public string Delete(CommentDelete comment)
        {
            if (comment.UserId != 0 && comment.Id != 0)
            {
                _worker.DeleteComment(comment.UserId, comment.Id);
                return "Success";
            }
            else
            {
                NotFound();
                return "Err: he is null ";
            }
        }
        /// <summary>
        /// Реализовано, но не используется
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPut(Name = "PutComment")]
        public string Put(CommentPut comment)
        {
            _worker.UpdateComment(comment.Id, comment.UserId, comment.ComDescription, comment.Rate);
            return "Success";
        }
    }
}
