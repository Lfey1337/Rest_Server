namespace Rest_Server.Services.DB_Models
{
    public class RouteComments
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RouteId { get; set; }
        public string ComDescription { get; set; }
        public double Rate { get; set; }
    }
}
