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
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; } = null!;
        public DbSet<TrackGenre> TrackGenres { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder){
            // PlaylistTrack
            modelBuilder.Entity<PlaylistTrack>().HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            // TrackGenre
            modelBuilder.Entity<TrackGenre>().HasKey(tg => new { tg.TrackId, tg.GenreId });

            // Playlist
            modelBuilder.Entity<Playlist>()
                .HasOne<Kind>(p => p.Kind)
                .WithMany(p => p.Playlists)
                .HasForeignKey(p => p.KindId);

            // Genre
            modelBuilder.Entity<Genre>().HasIndex(g => g.Name).IsUnique();

            // Kind
            modelBuilder.Entity<Kind>().HasIndex(k => k.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}