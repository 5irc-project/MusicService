using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MusicService.Models
{
    [Table("t_e_genre_gen")]
    public class Genre
    {
        [Column("gen_id")]
        public int GenreId { get; set; }

        [Column("gen_name")]
        public string? Name { get; set; }

        public virtual ICollection<Music>? Musics { get; set; }
    }
}