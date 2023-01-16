using MusicService.DTOs;

namespace MusicService.RestConsumers
{
    public class MLService
    {
         private static HttpClient client = new HttpClient();

        public static async Task<string> PredictGenre(List<TrackDTO> listTracks)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:8000/genre", listTracks);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("aaa");
                    // dto = await response.Content.ReadAsAsync<UserDTO>();
                    // return dto;
                }
            } 
            catch (Exception e)
            {
            }
            return "";
        }
    }
}