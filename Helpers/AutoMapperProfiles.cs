using AutoMapper;
using MusicService.DTOs;
using MusicService.Models;

namespace MusicService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Track, TrackDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistDTO>().ReverseMap();
            CreateMap<Kind, KindDTO>().ReverseMap();
        }
        
    }
}