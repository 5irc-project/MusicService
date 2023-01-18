using MusicService.DTOs;

namespace MusicService.HttpClient
{
    public class MLHttpClient : IMLHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string BASE_PATH = "";
        private readonly string GET_GENRE = "genre";

        public MLHttpClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            BASE_PATH = configuration["Http:MLService:Host"];
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GenreDTO?> PredictGenre(List<TrackMachineLearningDTO> listTracks)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonData = new Dictionary<String, List<TrackMachineLearningDTO>>();
            jsonData.Add("data", listTracks);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(BASE_PATH + GET_GENRE, jsonData);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<GenreDTO>();
            }

            throw new Exception("There was an error while predicting the genre");
        }
    }
}