using AutoMapper;
using MusicService.DTOs;
using MusicService.Models;

namespace MusicService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Track, TrackDTO>().ReverseMap();
            CreateMap<Track, TrackGetDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistGetDTO>().ReverseMap();
            CreateMap<Kind, KindDTO>().ReverseMap();
            CreateMap<PlaylistTrack, PlaylistTrackDTO>().ReverseMap();
            CreateMap<TrackGenre, TrackGenreDTO>().ReverseMap();

        }
        
    }
}