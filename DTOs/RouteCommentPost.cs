namespace Rest_Server.DTOs
{
    public class RouteCommentPost
    {
        public int RouteId { get; set; }
        public int UserId { get; set; }
        public string ComDescription { get; set; }
        public double Rate { get; set; }
    }
}
