using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicService.Models
{

    public class Kind
    {
        public int KindId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}