namespace Rest_Server.DTOs
{
    public class Route
    {
        public int Id { get; set; }
        public string RouteTitle { get; set; }
        public string RouteDesc { get; set; }
        public string RouteImg { get; set; }
        public string Extension { get; set; }
        public double RouteRate { get; set; }
        public int RouteReviews { get; set; }
        public bool IsVisited { get; set; }
        public bool IsLiked { get; set; }
    }
}
