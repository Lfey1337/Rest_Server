namespace Rest_Server.DTOs
{
    public class LikededRoutePut
    {
        public int UserId { get; set; }
        public int RouteId { get; set; }
        public bool IsLiked { get; set; }
    }
}
