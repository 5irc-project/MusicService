using AutoMapper;
using MusicService.DTOs;
using MusicService.Models;

namespace MusicService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Music, MusicDTO>();
            CreateMap<Genre, GenreDTO>();
        }
        
    }
}