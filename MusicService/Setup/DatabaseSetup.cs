using MusicService.DTOs;
using MusicService.Services.Interfaces;

namespace MusicService.Setup
{
    public class DatabaseSetup
    {   
        private readonly IGenreService _genreService;
        private readonly ITrackService _trackService;

        public DatabaseSetup(IGenreService genreService, ITrackService trackService){
            _genreService = genreService;
            _trackService = trackService;
        }

        public void Init(){
            List<TrackWithGenresDTO> ltwgDTO = new List<TrackWithGenresDTO>();
            List<GenreDTO> lgDTO = new List<GenreDTO>();
            using (StreamReader reader = new StreamReader("/home/sah0428/School/PROJET_FIN_ANNEE/MusicService/Data/cleaned_music_genre.csv"))
            {
                while(!reader.EndOfStream){
                    var line = reader.ReadLine();
                    if (line != null){
                        var values = line.Split(",");
                        
                        try {
                            GenreDTO gDTO = new GenreDTO {
                                Name = values[17]
                            };
                            TrackWithGenresDTO twgDTO = new TrackWithGenresDTO {
                                ArtistName = values[1],
                                TrackName = values[2],
                                Popularity = float.Parse(values[3].Replace(".", ",")),    
                                Acousticness = float.Parse(values[4].Replace(".", ",")),
                                Danceability = float.Parse(values[5].Replace(".", ",")),     
                                DurationMs = float.Parse(values[6].Replace(".", ",")),
                                Energy = float.Parse(values[7].Replace(".", ",")),
                                Instrumentalness = float.Parse(values[8].Replace(".", ",")),
                                Key = values[9],
                                Liveness = float.Parse(values[10].Replace(".", ",")),
                                Loudness = float.Parse(values[11].Replace(".", ",")),
                                Speechiness = float.Parse(values[13].Replace(".", ",")),
                                Tempo = float.Parse(values[14].Replace(".", ",")),
                                Valence = float.Parse(values[16].Replace(".", ",")),
                                Genres = new List<GenreDTO> {
                                    gDTO
                                }
                            };
                            if (!lgDTO.Any(g=> g.Name == gDTO.Name)){
                                lgDTO.Add(gDTO);
                            }
                            ltwgDTO.Add(twgDTO);
                        }catch{
                            continue;
                        }
                    }
                }
                lgDTO.ForEach(gDTO => {
                    _genreService.PostGenre(gDTO);
                });
                ltwgDTO.ForEach(twgDTO => {
                    _trackService.PostTrackWithGenres(twgDTO);
                });
            }
        }        
    }
}