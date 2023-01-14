namespace MusicService.DTOs
{
    public class GenreDTO
    {
        public int GenreId { get; set; }
        public string Name { get; set; } = null!;

        public override bool Equals(Object obj){
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                GenreDTO g = (GenreDTO) obj;
                return (GenreId == g.GenreId) && (Name == g.Name);
            }
        }
    }
}