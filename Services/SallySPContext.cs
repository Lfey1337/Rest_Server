using Microsoft.EntityFrameworkCore;
using Rest_Server.Services.DB_Models;
using Rest_Server.DTOs;

namespace Rest_Server.Services
{
    public class SallySPContext : DbContext
    {
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Passwords> Passwords { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Avatars> Avatars { get; set; }
        public DbSet<ContentOfRoute> RouteContent { get; set; }
        public DbSet<TitleImageRoute> TitleImageRoute { get; set; }
        public DbSet<LikesAndVisits> LikesAndVisits { get; set; }
        public DbSet<RouteComments> RouteComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=Lfeys_Den;Initial Catalog=SallySP;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True");
            optionsBuilder.UseSqlServer(@"Data Source=213.189.201.168;Initial Catalog=SallySP;Persist Security Info=True;User ID=degra;Password=5M1qecOW;MultipleActiveResultSets=True;TrustServerCertificate=True");
        }

        public DbSet<Rest_Server.DTOs.Comment> Comment { get; set; } = default!;
    }
}
