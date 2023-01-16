namespace MusicService.DTOs
{
    #pragma warning disable CS0659
    public class GenreDTO
    {
        public int GenreId { get; set; }
        public string Name { get; set; } = null!;

        #pragma warning disable CS8765
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
        #pragma warning restore CS8765
    }
    #pragma warning restore CS0659
}