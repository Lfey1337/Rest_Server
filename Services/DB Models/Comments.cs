namespace Rest_Server.Services.DB_Models
{
    public class Comments
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ComDescription { get; set; }
        public double Rate { get; set; }
    }
}
