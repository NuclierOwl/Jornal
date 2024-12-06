using data.RemoteData.RemoteDataBase.DAO;
using Microsoft.EntityFrameworkCore;

namespace remoteData.RemoteDataBase
{
    public class RemoteDatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=45.67.56.214;Port=5421;Username=user16;Password=dZ28IVE5;Database=user16");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupDao>().HasKey(group => group.Id);
            modelBuilder.Entity<GroupDao>().Property(group => group.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserDao>().HasKey(user => user.Guid);
            modelBuilder.Entity<UserDao>().Property(user => user.Guid).ValueGeneratedOnAdd();

            modelBuilder.Entity<PresenceDao>()
    .HasKey(presence => presence.Id); 

            modelBuilder.Entity<PresenceDao>()
                .Property(presence => presence.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PresenceDao>()
                .HasOne(presence => presence.User)
                .WithMany(user => user.Presences)
                .HasForeignKey(presence => presence.UserGuid);

        }

        public DbSet<GroupDao> groups { get; set; }
        public DbSet<UserDao> users { get; set; }
        public DbSet<PresenceDao> presence { get; set; }
    }
}
