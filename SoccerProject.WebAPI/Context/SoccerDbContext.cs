using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Entities;

namespace SoccerProject.WebAPI.Context
{
    public class SoccerDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=CEMRE\\SQLEXPRESS;Database=SoccerProjectDb;Integrated Security=true;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Fixture>()
                .HasOne(f => f.HomeTeam)
                .WithMany()
                .HasForeignKey(f => f.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Fixture>()
                .HasOne(f => f.AwayTeam)
                .WithMany()
                .HasForeignKey(f => f.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            // Futbolcu bir takıma bağlı; takım silinirken futbolcular otomatik silinmesin (Restrict).
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kariyer kayıtları futbolcuya bağlı; futbolcu silinince kariyeri de silinsin (Cascade).
            modelBuilder.Entity<PlayerCareer>()
                .HasOne(c => c.Player)
                .WithMany(p => p.Careers)
                .HasForeignKey(c => c.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Fikstür opsiyonel olarak gerçek bir stadyuma bağlanır (stadyum silinince maç silinmesin).
            modelBuilder.Entity<Fixture>()
                .HasOne(f => f.StadiumRef)
                .WithMany()
                .HasForeignKey(f => f.StadiumId)
                .OnDelete(DeleteBehavior.NoAction);

            // Maç olayı opsiyonel olarak gerçek bir futbolcuya bağlanır.
            modelBuilder.Entity<MatchEvent>()
                .HasOne(e => e.Player)
                .WithMany()
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<MatchEvent> MatchEvents { get; set; }
        public DbSet<MatchStatic> MatchStatics { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerCareer> PlayerCareers { get; set; }
    }
}
