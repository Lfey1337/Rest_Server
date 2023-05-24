namespace Rest_Server.Services.DB_Models
{
    public class LikesAndVisits
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int UserId { get; set; }
        public bool IsLiked { get; set; }
        public bool IsVisited { get; set; }
    }
}
