using MusicService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System;

namespace MusicServiceInit
{
    class Program
    {
        private static string ENVIRONMENT = GetEnvironment();
        private static string PathToCSV = "./cleaned_music_genre.csv";
        private static MusicServiceDBContext? _context;
        static void Main(string[] args)
        {
            #pragma warning disable CS8602
            var services = new ServiceCollection();
            var configurationRoot = ConfigureJsonEnv();
            Console.WriteLine(configurationRoot.GetConnectionString("MusicServiceDBContext"));
            services.AddDbContext<MusicServiceDBContext>(opt => opt.UseNpgsql(configurationRoot.GetConnectionString("MusicServiceDBContext")));
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetService<MusicServiceDBContext>();
            try{
                _context.Database.Migrate();
            }catch(Exception e){
                throw e; 
            }

            if (_context.Genres.ToList().Count == 0 && _context.Tracks.ToList().Count == 0 && _context.Kinds.ToList().Count == 0) {
                var dataFromCSV = extractDataFromCSV();
                List<Genre> listGenreToAdd = dataFromCSV.Item1;
                List<Track> listTrackToAdd = dataFromCSV.Item2;
                List<TrackGenre> listTrackGenreToAdd = dataFromCSV.Item3;

                _context.Genres.AddRange(listGenreToAdd);
                _context.Tracks.AddRange(listTrackToAdd);
                _context.TrackGenres.AddRange(listTrackGenreToAdd);
                _context.Kinds.AddRange(new List<Kind> {
                    new Kind { KindId = 1, Name = "Generated"},
                    new Kind { KindId = 2, Name = "Manual" },
                    new Kind { KindId = 3, Name = "Favorite" }
                });

                _context.SaveChanges();
                #pragma warning restore CS8602
            }
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
                        

                        for (int k =0; k < values.Length; k++){
                            if (values[k].Contains('"') || values[k].Contains('\\')){
                                values[k] = values[k].Replace('"', ' ');
                                values[k] = values[k].Replace('\\', ' ');
                                values[k] = values[k].Trim();
                            }
                        }

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

                            #pragma warning disable CS8602, CS8601
                            TrackGenre trackGenre = new TrackGenre {
                                Genre = listGenre.Find(g => g.Name == values[17]),
                                GenreId = listGenre.Find(g => g.Name == values[17]).GenreId,
                                Track = t,
                                TrackId = t.TrackId
                            };
                            #pragma warning restore CS8602, CS8601

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

        private static string GetEnvironment()
        {

            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine("From method : " + environment);
                return environment == null ? "Production" : environment;
            }
            catch (Exception)
            {
                return "Production";
            }
        }

        private static IConfigurationRoot ConfigureJsonEnv()
        {
            Console.WriteLine("From class : " + ENVIRONMENT);
            var builder = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", true, true)
            .AddJsonFile($"appsettings.{ENVIRONMENT}.json", true, true)
            .AddEnvironmentVariables();
            var configurationRoot = builder.Build();

            return configurationRoot;
        }
    }
}