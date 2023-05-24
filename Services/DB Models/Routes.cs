namespace Rest_Server.Services.DB_Models
{
    public class Routes
    {
        public int Id { get; set; }
        public string RouteTitle { get; set; }
        public string RouteDesc { get; set; }
        public double RouteRate { get; set; }
        public int RouteReviews { get; set; }
    }
}
