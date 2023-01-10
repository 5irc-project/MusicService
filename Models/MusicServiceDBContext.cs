using Microsoft.EntityFrameworkCore;

namespace MusicService.Models
{
    public class MusicServiceDBContext : DbContext
    {
        public MusicServiceDBContext(DbContextOptions<MusicServiceDBContext> options) : base(options) { }

        public DbSet<Music> Musics { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Mood> Moods { get; set; } = null!;
        public DbSet<Playlist> Playlists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}