using MusicService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace MusicServiceInit
{
    class Program
    {
        private static string PathToCSV = "./cleaned_music_genre.csv";
        private static MusicServiceDBContext? _context;
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<MusicServiceDBContext>(opt => opt.UseNpgsql("Server=localhost;port=8003;Database=MusicServiceDatabase;uid=root;password=root;"));

            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetService<MusicServiceDBContext>();

            var dataFromCSV = extractDataFromCSV();
            List<Genre> listGenreToAdd = dataFromCSV.Item1;
            List<Track> listTrackToAdd = dataFromCSV.Item2;
            List<TrackGenre> listTrackGenreToAdd = dataFromCSV.Item3;

            _context.AddRange(listGenreToAdd);
            _context.AddRange(listTrackToAdd);
            _context.AddRange(listTrackGenreToAdd);

            _context.SaveChanges();
        }

        private static (List<Genre>, List<Track>, List<TrackGenre>) extractDataFromCSV(){
            List<Genre> listGenre = new List<Genre>();
            List<Track> listTrack = new List<Track>();
            List<TrackGenre> listTrackGenre = new List<TrackGenre>();
            int i = 1, j = 1;
            using (StreamReader reader = new StreamReader(PathToCSV))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                        try
                        {
                            Track t = new Track {
                                TrackId = j,
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
                            };

                            if (!listGenre.Any(g => g.Name == values[17])){
                                listGenre.Add(new Genre {
                                    GenreId = i,
                                    Name = values[17]
                                });
                                i++;
                            }

                            TrackGenre trackGenre = new TrackGenre {
                                Genre = listGenre.Find(g => g.Name == values[17]),
                                GenreId = listGenre.Find(g => g.Name == values[17]).GenreId,
                                Track = t,
                                TrackId = t.TrackId
                            };

                            listTrack.Add(t);
                            listTrackGenre.Add(trackGenre);

                            j++;
                        } catch {
                            continue;
                        }
                    }
                }
            }
            return (listGenre, listTrack, listTrackGenre);
        }
    }
}