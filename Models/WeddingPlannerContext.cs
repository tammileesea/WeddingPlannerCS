using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models {
    public class WeddingPlannerContext : DbContext {
        public WeddingPlannerContext(DbContextOptions options) : base(options) {}
        public DbSet<User> users {get;set;}
        public DbSet<Wedding> weddings {get;set;}
        public DbSet<WeddingGuest> weddingGuests {get;set;}
    }
}