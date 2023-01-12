using Microsoft.EntityFrameworkCore;

namespace MusicService.Models
{
    public class MusicServiceDBContext : DbContext
    {
        public MusicServiceDBContext(DbContextOptions<MusicServiceDBContext> options) : base(options) { }

        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Kind> Kinds { get; set; } = null!;
        public DbSet<Playlist> Playlists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.HasDefaultSchema("public");

            // modelBuilder.Entity<Mood>().HasData(
            //     new Mood(-1, "Mood1"),
            //     new Mood(-2, "Mood2")
            // );

            // modelBuilder.Entity<Genre>().HasData(
            //     new Genre(-1, "Genre1"),
            //     new Genre(-2, "Genre2"),
            //     new Genre(-3, "Genre3"),
            //     new Genre(-4, "Genre4")
            // );

            base.OnModelCreating(modelBuilder);
        }
    }
}