using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<TrackGenre>? TrackGenres { get; set; }
    }
}