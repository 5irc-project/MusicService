using AutoMapper;
using MusicService.DTOs;
using MusicService.Models;

namespace MusicService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Track, TrackDTO>().ReverseMap();
            CreateMap<Track, TrackWithGenresDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistWithTracksDTO>().ReverseMap();
            CreateMap<Kind, KindDTO>().ReverseMap();
        }
    }
}