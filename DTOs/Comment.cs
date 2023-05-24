namespace Rest_Server.DTOs
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Nickname { get; set; }
        /// <summary>
        /// In Base64
        /// </summary>
        public string Avatar { get; set; }
        public string Extension { get; set; }
        public string ComDescription { get; set; }
        public double Rate { get; set; }
    }
}
