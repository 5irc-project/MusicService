using Microsoft.AspNetCore.Mvc;
using MusicService.DTOs;
using System.Net;

namespace MusicService.HttpClient
{
    public interface IMLHttpClient
    {
        public Task<GenreDTO?> PredictGenre(List<TrackMachineLearningDTO> listTracks);
    }

}