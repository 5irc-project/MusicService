using MusicService.Models;
using MusicService.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using MusicService.Services.Implementations;
using AutoMapper;
using MusicService.Helpers;
using MusicService.DTOs;
using MusicService.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MusicServiceTest.Controller
{
    [TestClass()]

    public class TrackControllerTests
    {
        private TrackController? _controller;
        private MusicServiceDBContext _context;
        private ITrackService _service;
        private IMapper _mapper;
        
        public TrackControllerTests(){
            var dbContextOptions = new DbContextOptionsBuilder<MusicServiceDBContext>().UseInMemoryDatabase("MusicServiceDBTest");
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfiles()));

            _context = new MusicServiceDBContext(dbContextOptions.Options);
            _context.Database.EnsureCreated();

            if (_context.Genres.Count() <= 0){
                _context.Genres.AddRange(
                    new Genre { GenreId = 1, Name = "GenreUn"},
                    new Genre { GenreId = 2, Name = "GenreDeux"}

                );
                _context.SaveChanges();
            }

            _mapper = mappingConfig.CreateMapper();  
            _service = new TrackService(_context, _mapper);
        }

        [TestInitialize]
        public void Setup(){
            _controller = new TrackController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _controller = null;
        }

        [TestMethod()]
        public void GetTracks_ReturnsOk()
        {
            // Arrange
            List<TrackDTO> listTrackToAdd = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1},
                new TrackDTO { TrackId = 2,  Acousticness = (float)0.2, ArtistName = "ArtistNameTwo", Danceability = (float)0.2, DurationMs = (float)200, Energy = (float)0.2, Instrumentalness = (float)0.2, Key = "A", Liveness = (float)0.2, Loudness = (float)0.2, Popularity = (float)20, Speechiness =(float)0.2, Tempo = (float)0.2, TrackName = "TrackNameTwo", Valence = (float)0.2},
            };
            List<TrackGenre> listTrackGenreToAdd = new List<TrackGenre>() {
                new TrackGenre { TrackId = listTrackToAdd[0].TrackId, GenreId = 1},
                new TrackGenre { TrackId = listTrackToAdd[0].TrackId, GenreId = 2},
                new TrackGenre { TrackId = listTrackToAdd[1].TrackId, GenreId = 1}
            };

            // Act
            listTrackToAdd.ForEach(trackToAdd => {
                _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            });
            listTrackGenreToAdd.ForEach(trackGenreToAdd => {
                _context.TrackGenres.Add(trackGenreToAdd);
            });
            _context.SaveChanges();
            List<TrackWithGenresDTO> listTrackToTestWithGenres = _controller.GetTracks().Result.Value;
            List<TrackDTO> listTrackToTestWithoutGenres = new List<TrackDTO>();
            listTrackToTestWithGenres.ForEach(trackWithGenre => {
                listTrackToTestWithoutGenres.Add(_mapper.Map<TrackDTO>(_mapper.Map<Track>(trackWithGenre)));      
            });

            // Assert
            CollectionAssert.AllItemsAreNotNull(listTrackToTestWithGenres);
            Assert.AreEqual(listTrackToTestWithGenres.Find(e => e.TrackId == 1).Genres.First(e => e.GenreId == 1), _mapper.Map<GenreDTO>(_context.Genres.Find(1)));
            Assert.AreEqual(listTrackToTestWithGenres.Find(e => e.TrackId == 1).Genres.First(e => e.GenreId == 2), _mapper.Map<GenreDTO>(_context.Genres.Find(2)));
            Assert.AreEqual(listTrackToTestWithGenres.Find(e => e.TrackId == 2).Genres.First(e => e.GenreId == 1), _mapper.Map<GenreDTO>(_context.Genres.Find(1)));
            Assert.AreEqual(listTrackToTestWithoutGenres.Find(e => e.TrackId == 1), listTrackToAdd.Find(e => e.TrackId == 1));
            Assert.AreEqual(listTrackToTestWithoutGenres.Find(e => e.TrackId == 2), listTrackToAdd.Find(e => e.TrackId == 2));
        }

        [TestMethod()]
        public void GetTrack_ReturnsOk()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -1,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            List<TrackGenre> listTrackGenreToAdd = new List<TrackGenre>() {
                new TrackGenre { TrackId = trackToAdd.TrackId, GenreId = 1},
                new TrackGenre { TrackId = trackToAdd.TrackId, GenreId = 2},
            };

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            listTrackGenreToAdd.ForEach(trackGenreToAdd => {
                _context.TrackGenres.Add(trackGenreToAdd);
            });
            _context.SaveChanges();
            TrackWithGenresDTO trackToTestWithGenres = _controller.GetTrack(trackToAdd.TrackId).Result.Value;
            TrackDTO trackToTestWithoutGenres = _mapper.Map<TrackDTO>(_mapper.Map<Track>(trackToTestWithGenres));

            // Assert
            Assert.IsInstanceOfType(trackToTestWithGenres, typeof(TrackWithGenresDTO));
            Assert.AreEqual(trackToTestWithoutGenres, trackToAdd);
            Assert.AreEqual(trackToTestWithGenres.Genres.First(e => e.GenreId == 1), _mapper.Map<GenreDTO>(_context.Genres.Find(1)));
            Assert.AreEqual(trackToTestWithGenres.Genres.First(e => e.GenreId == 2), _mapper.Map<GenreDTO>(_context.Genres.Find(2)));
        }

        [TestMethod()]
        public void GetTrack_ReturnsNotFound()
        {
            // Act
            var actionToTest = (NotFoundObjectResult)_controller.GetTrack(-1000).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Track found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void GetTrackByQueryName_ReturnsOk()
        {
            // Arrange
            List<TrackDTO> listTrackToAdd = new List<TrackDTO>() {
                new TrackDTO { TrackId = -101,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1},
                new TrackDTO { TrackId = -102,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1},
                new TrackDTO { TrackId = -103,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1},
                new TrackDTO { TrackId = -104,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "Bonjour", Valence = (float)0.1},
            };

            // Act
            listTrackToAdd.ForEach(trackToAdd => {
                _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            });
            _context.SaveChanges();
            List<TrackWithGenresDTO> listTrackToTestWithGenres = _controller.GetTracksByNameQuery("Track").Result.Value;

            // Assert
            Assert.AreEqual(listTrackToTestWithGenres.Count, 3);
        }

        [TestMethod()]
        public void PutTrack_ReturnsOk()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -2,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            TrackDTO trackToPut = new TrackDTO { TrackId = -2,  Acousticness = (float)0.2, ArtistName = "ArtistNameOneAfterPut", Danceability = (float)0.2, DurationMs = (float)100, Energy = (float)0.2, Instrumentalness = (float)0.2, Key = "A", Liveness = (float)0.2, Loudness = (float)0.2, Popularity = (float)20, Speechiness =(float)0.2, Tempo = (float)0.2, TrackName = "TrackNameOneAfterPut", Valence = (float)0.2};

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.PutTrack(trackToPut.TrackId, trackToPut).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Tracks.Find(trackToAdd.TrackId).TrackName, trackToPut.TrackName);         
        }

        [TestMethod()]
        public void PutTrack_ReturnsNotFound()
        {
            // Arrange
            TrackDTO trackToPut = new TrackDTO { TrackId = -1000,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.PutTrack(trackToPut.TrackId, trackToPut).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Track found with ID = {0}", trackToPut.TrackId));         
        }

        [TestMethod()]
        public void PutTrack_ReturnsBadRequest()
        {
            // Arrange
            TrackDTO trackToPut = new TrackDTO { TrackId = -1000,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};

            // Act
            var actionToTest = _controller.PutTrack(-1001, trackToPut).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(BadRequestResult));         
        }

        [TestMethod()]
        public void PostTrack_ReturnsOk()
        {
            // Arrange
            TrackDTO trackToPost = new TrackDTO { TrackId = -3,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};

            // Act
            var actionToTest = _controller.PostTrack(trackToPost).Result.Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(CreatedAtActionResult));
            Assert.AreEqual(_mapper.Map<TrackDTO>(_context.Tracks.Find(trackToPost.TrackId)), trackToPost);        
        }

        [TestMethod()]
        public void PostTrack_ReturnsAlreadyExists_Id()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -4,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            TrackDTO trackToPost = new TrackDTO { TrackId = -4,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (BadRequestObjectResult)_controller.PostTrack(trackToPost).Result.Result;
            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("Track with ID = {0} already exists", trackToPost.TrackId));        
        }

        [TestMethod()]
        public void PostTrackWithGenres_ReturnsOk()
        {
            // Arrange
            TrackWithGenresDTO trackWithGenresToPost = new TrackWithGenresDTO { TrackId = -5, Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1,Genres = new List<GenreDTO>() { _mapper.Map<GenreDTO>(_context.Genres.Find(1)), _mapper.Map<GenreDTO>(_context.Genres.Find(2)) }};

            // Act
            var actionToTest = _controller.PostTrackWithGenres(trackWithGenresToPost).Result.Result;
            Track trackCreated = _context.Tracks.Find(trackWithGenresToPost.TrackId);
            TrackWithGenresDTO trackWithGenresCreated = _mapper.Map<TrackWithGenresDTO>(trackCreated);
            trackWithGenresCreated.Genres = new List<GenreDTO>();
            trackCreated.TrackGenres.ToList().ForEach(e => {
                trackWithGenresCreated.Genres.Add(_mapper.Map<GenreDTO>(e.Genre));
            });

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(CreatedAtActionResult));
            Assert.AreEqual(trackWithGenresToPost, trackWithGenresCreated);      
        }

        [TestMethod()]
        public void PostTrackWithGenres_ReturnsNotFound()
        {
            // Arrange
            TrackWithGenresDTO trackWithGenresToPost = new TrackWithGenresDTO { TrackId = -6, Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1,Genres = new List<GenreDTO>() { _mapper.Map<GenreDTO>(_context.Genres.Find(1)), new GenreDTO() { GenreId = 4, Name = "Fake"}}};

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.PostTrackWithGenres(trackWithGenresToPost).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, "At least one of the Genre given wasn't found");    
        }

        [TestMethod()]
        public void PostTrackWithGenres_ReturnsAlreadyExists_Id()
        {
            // Arrange
            TrackWithGenresDTO trackWithGenresToAdd = new TrackWithGenresDTO { TrackId = -7, Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1,Genres = new List<GenreDTO>() { _mapper.Map<GenreDTO>(_context.Genres.Find(1)), _mapper.Map<GenreDTO>(_context.Genres.Find(2)) }};
            TrackWithGenresDTO trackWithGenresToPost = new TrackWithGenresDTO { TrackId = -7, Acousticness = (float)0.1, ArtistName = "ArtistNameOneBis", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1,Genres = new List<GenreDTO>() { _mapper.Map<GenreDTO>(_context.Genres.Find(1)), _mapper.Map<GenreDTO>(_context.Genres.Find(2)) }};

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackWithGenresToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (BadRequestObjectResult)_controller.PostTrackWithGenres(trackWithGenresToPost).Result.Result;
            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("Track with ID = {0} already exists", trackWithGenresToPost.TrackId));        
        }

        [TestMethod()]
        public void DeleteTrack_ReturnsOk()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -8,  Acousticness = (float)0.1, ArtistName = "ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.DeleteTrack(trackToAdd.TrackId).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Tracks.Find(trackToAdd.TrackId), null);
        }

        [TestMethod()]
        public void AddGenresToTrack_ReturnsOk()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -9,  Acousticness = (float)0.1, ArtistName = "   ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            List<GenreDTO> listGenreToAddToTrack = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1},
                new GenreDTO { GenreId = 2},
            };

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.AddGenresToTrack(trackToAdd.TrackId, listGenreToAddToTrack).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.IsNotNull(_context.TrackGenres.AsNoTracking().FirstOrDefault(tg => tg.TrackId == trackToAdd.TrackId && tg.GenreId == 1));
            Assert.IsNotNull(_context.TrackGenres.AsNoTracking().FirstOrDefault(tg => tg.TrackId == trackToAdd.TrackId && tg.GenreId == 2));
        }

        [TestMethod()]
        public void AddGenresToTrack_ReturnsNotFound_Track()
        {
            // Arrange
            List<GenreDTO> listGenreToAddToTrack = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1},
                new GenreDTO { GenreId = 2},
            };

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.AddGenresToTrack(-1000, listGenreToAddToTrack).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Track found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void AddGenresToTrack_ReturnsNotFound_Genre()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -10,  Acousticness = (float)0.1, ArtistName = "   ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            List<GenreDTO> listGenreToAddToTrack = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1},
                new GenreDTO { GenreId = 1000},
            };

            var test = _context.TrackGenres;

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (NotFoundObjectResult)_controller.AddGenresToTrack(trackToAdd.TrackId, listGenreToAddToTrack).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("At least one of the Genre given wasn't found"));
        }

        [TestMethod()]
        public void RemoveGenresFromTrack_ReturnsOk()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -11,  Acousticness = (float)0.1, ArtistName = "   ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            List<TrackGenre> listTrackGenreToAdd = new List<TrackGenre>() {
                new TrackGenre { TrackId = trackToAdd.TrackId, GenreId = 1},
                new TrackGenre { TrackId = trackToAdd.TrackId, GenreId = 2},
            };
            List<GenreDTO> listGenreToRemoveFromTrack = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1},
            };

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            listTrackGenreToAdd.ForEach(trackGenreToAdd => {
                _context.TrackGenres.Add(trackGenreToAdd);
            });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.RemoveGenresFromTrack(trackToAdd.TrackId, listGenreToRemoveFromTrack).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.IsNull(_context.TrackGenres.AsNoTracking().FirstOrDefault(tg => tg.TrackId == trackToAdd.TrackId && tg.GenreId == 1));
            Assert.IsNotNull(_context.TrackGenres.AsNoTracking().FirstOrDefault(tg => tg.TrackId == trackToAdd.TrackId && tg.GenreId == 2));
        }

        [TestMethod()]
        public void RemoveGenresFromTrack_ReturnsNotFound_Track()
        {
            // Arrange
            List<GenreDTO> listGenreToRemoveFromTrack = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1}
            };

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.RemoveGenresFromTrack(-1000, listGenreToRemoveFromTrack).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Track found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void RemoveGenresFromTrack_ReturnsNotFound_Genre()
        {
            // Arrange
            TrackDTO trackToAdd = new TrackDTO { TrackId = -12,  Acousticness = (float)0.1, ArtistName = "   ArtistNameOne", Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackNameOne", Valence = (float)0.1};
            List<TrackGenre> listTrackGenreToAdd = new List<TrackGenre>() {
                new TrackGenre { TrackId = trackToAdd.TrackId, GenreId = 1},
                new TrackGenre { TrackId = trackToAdd.TrackId, GenreId = 2},
            };
            List<GenreDTO> listGenreToRemoveFromTrack = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1000},
            };

            var test = _context.TrackGenres;

            // Act
            _context.Tracks.Add(_mapper.Map<Track>(trackToAdd));
            listTrackGenreToAdd.ForEach(trackGenreToAdd => {
                _context.TrackGenres.Add(trackGenreToAdd);
            });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (NotFoundObjectResult)_controller.AddGenresToTrack(trackToAdd.TrackId, listGenreToRemoveFromTrack).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("At least one of the Genre given wasn't found"));
        }
    }
}