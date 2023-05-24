namespace Rest_Server.DTOs
{
    public class VisitedRoutePut
    {
        public int UserId { get; set; }
        public int RouteId { get; set; }
        public bool IsVisited { get; set; }
    }
}
