using MusicService.DTOs;

namespace MusicService.RestConsumer
{
    public class MLService
    {
         private static HttpClient client = new HttpClient();

        public static async Task<GenreDTO?> PredictGenre(List<TrackMachineLearningDTO> listTracks)
        {
            try
            {
                var jsonData = new Dictionary<String, List<TrackMachineLearningDTO>>();
                jsonData.Add("data", listTracks);
                HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:8000/genre", jsonData);
                if (response.IsSuccessStatusCode) {
                    return await response.Content.ReadFromJsonAsync<GenreDTO>();
                }else {
                    throw new Exception("There was an error while predicting the genre");
                }
            }
            catch (Exception e) {
                throw e;
            }
        }
    }
}