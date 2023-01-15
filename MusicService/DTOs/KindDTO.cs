namespace MusicService.DTOs
{
    public class KindDTO
    {
        public int KindId { get; set; }
        public string Name { get; set; } = null!;

        public override bool Equals(Object obj){
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                KindDTO g = (KindDTO) obj;
                return (KindId == g.KindId) && (Name == g.Name);
            }
        }
    }
}