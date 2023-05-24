namespace Rest_Server.DTOs
{
    public class CommentPut
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ComDescription { get; set; }
        public double Rate { get; set; }
    }
}
