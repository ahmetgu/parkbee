using Microsoft.EntityFrameworkCore;

namespace ApI.Data
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext()
        {
        }

        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
        : base(options)
        {
        }

        public DbSet<Garage> Garages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }    
        public DbSet<SessionHistory> SessionHistories { get; set; }
    }
}