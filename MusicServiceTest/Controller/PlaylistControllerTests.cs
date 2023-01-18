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

#pragma warning disable CS8602, CS8600, CS8604, CS8625
namespace MusicServiceTest.Controller
{
    [TestClass()]
    public class PlaylistControllerTests
    {
        private PlaylistController? _controller;
        private MusicServiceDBContext _context;
        private IPlaylistService _service;
        private ITrackService _trackService;
        private IMapper _mapper;
        
        public PlaylistControllerTests(){
            var dbContextOptions = new DbContextOptionsBuilder<MusicServiceDBContext>().UseInMemoryDatabase("MusicServiceDBTest");
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfiles()));

            _context = new MusicServiceDBContext(dbContextOptions.Options);
            _context.Database.EnsureCreated();

            if (_context.Kinds.Count() <= 0 && _context.Tracks.Count() <= 0 && _context.Genres.Count() <= 0){
                _context.Genres.AddRange(
                    new Genre { GenreId = 1, Name = "GenreUn"},
                    new Genre { GenreId = 2, Name = "GenreDeux"}

                );
                _context.Kinds.AddRange(
                    new Kind { KindId = 1, Name = "KindUn"},
                    new Kind { KindId = 2, Name = "KindDeux"},
                    new Kind { KindId = 3, Name = "KindDeux"}
                );
                for (int i = 1; i<6; i++){
                    _context.Tracks.Add(new Track(){ TrackId = i,  Acousticness = (float)0.1, ArtistName = "ArtistName" + i, Danceability = (float)0.1, DurationMs = (float)100, Energy = (float)0.1, Instrumentalness = (float)0.1, Key = "A", Liveness = (float)0.1, Loudness = (float)0.1, Popularity = (float)10, Speechiness =(float)0.1, Tempo = (float)0.1, TrackName = "TrackName" + i, Valence = (float)0.1});
                }
                _context.TrackGenres.AddRange(
                    new TrackGenre() { TrackId = 1, GenreId = 1 },
                    new TrackGenre() { TrackId = 1, GenreId = 2 },
                    new TrackGenre() { TrackId = 2, GenreId = 1 },
                    new TrackGenre() { TrackId = 3, GenreId = 2 },
                    new TrackGenre() { TrackId = 4, GenreId = 2 },
                    new TrackGenre() { TrackId = 5, GenreId = 1 },
                    new TrackGenre() { TrackId = 5, GenreId = 2 }
                );
                _context.SaveChanges();
            }

            _mapper = mappingConfig.CreateMapper(); 
            _trackService = new TrackService(_context, _mapper); 
            _service = new PlaylistService(_context, _mapper, _trackService, null, null);

        }

        [TestInitialize]
        public void Setup(){
            _controller = new PlaylistController(_service, null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _controller = null;
            _context.Database.EnsureDeleted();
        }

        // [TestMethod()]
        // public void GetPlaylists_ReturnsOk()
        // {
        //     // TODO : I have to map genres to track to take trackGenresDTO I think

        //     // Arrange
        //     List<PlaylistDTO> listPlaylistToAdd = new List<PlaylistDTO>() {
        //         new PlaylistDTO { PlaylistId = 1, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" },
        //         new PlaylistDTO { PlaylistId = 2, KindId = 2, UserId = 1, PlaylistName = "PlaylistOne" }
        //     };
        //     List<PlaylistTrack> listPlaylistTrackToAdd = new List<PlaylistTrack>() {
        //         new PlaylistTrack { PlaylistId = listPlaylistToAdd[0].PlaylistId, TrackId = 1},
        //         new PlaylistTrack { PlaylistId = listPlaylistToAdd[0].PlaylistId, TrackId = 2},
        //         new PlaylistTrack { PlaylistId = listPlaylistToAdd[1].PlaylistId, TrackId = 2},
        //         new PlaylistTrack { PlaylistId = listPlaylistToAdd[1].PlaylistId, TrackId = 4},
        //         new PlaylistTrack { PlaylistId = listPlaylistToAdd[1].PlaylistId, TrackId = 5}

        //     };

        //     // Act
        //     listPlaylistToAdd.ForEach(playlistToAdd => {
        //         _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
        //     });
        //     listPlaylistTrackToAdd.ForEach(playlistGenreToAdd => {
        //         _context.PlaylistTracks.Add(playlistGenreToAdd);
        //     });
        //     _context.SaveChanges();
        //     List<PlaylistWithTracksDTO> listPlaylistToTestWithTracks = _controller.GetPlaylists().Result.Value;
        //     List<PlaylistDTO> listPlaylistToTestWithoutTracks = new List<PlaylistDTO>();
        //     listPlaylistToTestWithTracks.ForEach(playlistWithTrack => {
        //         listPlaylistToTestWithoutTracks.Add(_mapper.Map<PlaylistDTO>(_mapper.Map<Playlist>(playlistWithTrack)));      
        //     });

        //     // Assert
        //     CollectionAssert.AllItemsAreNotNull(listPlaylistToTestWithTracks);
        //     Assert.AreEqual(listPlaylistToTestWithTracks.Find(e => e.PlaylistId == 1).Tracks.First(t => t.TrackId == 1), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(1)));
        //     Assert.AreEqual(listPlaylistToTestWithTracks.Find(e => e.PlaylistId == 1).Tracks.First(t => t.TrackId == 2), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(2)));
        //     Assert.AreEqual(listPlaylistToTestWithTracks.Find(e => e.PlaylistId == 2).Tracks.First(t => t.TrackId == 2), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(2)));
        //     Assert.AreEqual(listPlaylistToTestWithTracks.Find(e => e.PlaylistId == 2).Tracks.First(t => t.TrackId == 4), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(4)));
        //     Assert.AreEqual(listPlaylistToTestWithTracks.Find(e => e.PlaylistId == 2).Tracks.First(t => t.TrackId == 5), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(5)));
        //     Assert.AreEqual(listPlaylistToTestWithoutTracks.Find(e => e.PlaylistId == 1), listPlaylistToAdd.Find(e => e.PlaylistId == 1));
        //     Assert.AreEqual(listPlaylistToTestWithoutTracks.Find(e => e.PlaylistId == 2), listPlaylistToAdd.Find(e => e.PlaylistId == 2));
        // }

        // [TestMethod()]
        // public void GetPlaylist_ReturnsOk()
        // {
        //     // TODO : I have to map genres to track to take trackGenresDTO I think

        //     // Arrange
        //     PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -1, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
        //     List<PlaylistTrack> listPlaylistTrackToAdd = new List<PlaylistTrack>() {
        //         new PlaylistTrack { PlaylistId = playlistToAdd.PlaylistId, TrackId = 1},
        //         new PlaylistTrack { PlaylistId = playlistToAdd.PlaylistId, TrackId = 4},
        //     };

        //     // Act
        //     _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
        //     listPlaylistTrackToAdd.ForEach(playlistTrackToAdd => {
        //         _context.PlaylistTracks.Add(playlistTrackToAdd);
        //     });
        //     _context.SaveChanges();
        //     PlaylistWithTracksDTO playlistToTestWithTracks = _controller.GetPlaylist(playlistToAdd.PlaylistId).Result.Value;
        //     PlaylistDTO playlistToTestWithoutTracks = _mapper.Map<PlaylistDTO>(_mapper.Map<Playlist>(playlistToTestWithTracks));

        //     // Assert
        //     Assert.IsInstanceOfType(playlistToTestWithTracks, typeof(PlaylistWithTracksDTO));
        //     Assert.AreEqual(playlistToTestWithoutTracks, playlistToAdd);
        //     Assert.AreEqual(playlistToTestWithTracks.Tracks.First(e => e.TrackId == 1), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(1)));
        //     Assert.AreEqual(playlistToTestWithTracks.Tracks.First(e => e.TrackId == 4), _mapper.Map<TrackWithGenresDTO>(_context.Tracks.Find(4)));
        // }

        [TestMethod()]
        public void GetPlaylist_ReturnsNotFound()
        {
            // Act
            var actionToTest = (NotFoundObjectResult)_controller.GetPlaylist(-1000).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Playlist found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void GetPlaylistByUser_ReturnsOk()
        {
            // Arrange
            List<PlaylistDTO> listPlaylistToAdd = new List<PlaylistDTO>() {
                new PlaylistDTO(){ PlaylistId = -101, KindId = 1, UserId = 1000, PlaylistName = "PlaylistOne" },
                new PlaylistDTO(){ PlaylistId = -102, KindId = 1, UserId = 1000, PlaylistName = "PlaylistOne" },
                new PlaylistDTO(){ PlaylistId = -103, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" },
                new PlaylistDTO(){ PlaylistId = -104, KindId = 1, UserId = 1000, PlaylistName = "PlaylistOne" }
            };

            // Act
            listPlaylistToAdd.ForEach(playlistToAdd => {
                _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            });
            _context.SaveChanges();
            List<PlaylistWithTracksDTO> listPlaylistToTestWithTracks = _controller.GetPlaylistsByUserId(1000).Result.Value;

            // Assert
            Assert.AreEqual(listPlaylistToTestWithTracks.Count, 3);
        }

        [TestMethod()]
        public void PutPlaylist_ReturnsOk()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -2, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
            PlaylistDTO playlistToPut = new PlaylistDTO(){ PlaylistId = -2, KindId = 1, UserId = 1, PlaylistName = "PlaylistOneModified" };

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.PutPlaylist(playlistToPut.PlaylistId, playlistToPut).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Playlists.Find(playlistToAdd.PlaylistId).PlaylistName, playlistToPut.PlaylistName);         
        }

        [TestMethod()]
        public void PutPlaylist_ReturnsNotFound()
        {
            // Arrange
            PlaylistDTO playlistToPut = new PlaylistDTO(){ PlaylistId = -1000, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.PutPlaylist(playlistToPut.PlaylistId, playlistToPut).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Playlist found with ID = {0}", playlistToPut.PlaylistId));         
        }

        [TestMethod()]
        public void PutPlaylist_ReturnsBadRequest()
        {
            // Arrange
            PlaylistDTO playlistToPut = new PlaylistDTO(){ PlaylistId = -1000, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };

            // Act
            var actionToTest = _controller.PutPlaylist(-1001, playlistToPut).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(BadRequestResult));         
        }

        [TestMethod()]
        public void PostPlaylist_ReturnsOk()
        {
            // Arrange
            PlaylistDTO playlistToPost = new PlaylistDTO(){ PlaylistId = -3, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };

            // Act
            var actionToTest = _controller.PostPlaylist(playlistToPost).Result.Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(CreatedAtActionResult));
            Assert.AreEqual(_mapper.Map<PlaylistDTO>(_context.Playlists.Find(playlistToPost.PlaylistId)), playlistToPost);        
        }

        [TestMethod()]
        public void PostPlaylist_ReturnsAlreadyExists_Id()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -4, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
            PlaylistDTO playlistToPost = new PlaylistDTO(){ PlaylistId = -4, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (BadRequestObjectResult)_controller.PostPlaylist(playlistToPost).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("Playlist with ID = {0} already exists", playlistToPost.PlaylistId));        
        }

        [TestMethod()]
        public void DeletePlaylist_ReturnsOk()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -5, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.DeletePlaylist(playlistToAdd.PlaylistId).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Playlists.Find(playlistToAdd.PlaylistId), null);
        }

        [TestMethod()]
        public void AddTracksToPlaylist_ReturnsOk()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -6, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
            List<TrackDTO> listTrackToAddToPlaylist = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1},
                new TrackDTO { TrackId = 2},
            };

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.AddTracksToPlaylist(playlistToAdd.PlaylistId, listTrackToAddToPlaylist).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.IsNotNull(_context.PlaylistTracks.AsNoTracking().FirstOrDefault(tg => tg.PlaylistId == playlistToAdd.PlaylistId && tg.TrackId == 1));
            Assert.IsNotNull(_context.PlaylistTracks.AsNoTracking().FirstOrDefault(tg => tg.PlaylistId == playlistToAdd.PlaylistId && tg.TrackId == 2));
        }

        [TestMethod()]
        public void AddTracksToPlaylist_ReturnsNotFound_Playlist()
        {
            // Arrange
            List<TrackDTO> listTrackToAddToPlaylist = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1},
                new TrackDTO { TrackId = 2},
            };

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.AddTracksToPlaylist(-1000, listTrackToAddToPlaylist).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Playlist found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void AddTracksToPlaylist_ReturnsNotFound_Track()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -7, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
            List<TrackDTO> listTrackToAddToPlaylist = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1},
                new TrackDTO { TrackId = 1000},
            };

            var test = _context.PlaylistTracks;

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (NotFoundObjectResult)_controller.AddTracksToPlaylist(playlistToAdd.PlaylistId, listTrackToAddToPlaylist).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("At least one of the Track given wasn't found"));
        }

        [TestMethod()]
        public void RemoveTracksFromPlaylist_ReturnsOk()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -8, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
            List<PlaylistTrack> listPlaylistTrackToAdd = new List<PlaylistTrack>() {
                new PlaylistTrack { PlaylistId = playlistToAdd.PlaylistId, TrackId = 1},
                new PlaylistTrack { PlaylistId = playlistToAdd.PlaylistId, TrackId = 2},
            };
            List<TrackDTO> listTrackToRemoveFromPlaylist = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1},
            };

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            listPlaylistTrackToAdd.ForEach(playlistTrackToAdd => {
                _context.PlaylistTracks.Add(playlistTrackToAdd);
            });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.RemoveTracksFromPlaylist(playlistToAdd.PlaylistId, listTrackToRemoveFromPlaylist).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.IsNull(_context.PlaylistTracks.AsNoTracking().FirstOrDefault(tg => tg.PlaylistId == playlistToAdd.PlaylistId && tg.TrackId == 1));
            Assert.IsNotNull(_context.PlaylistTracks.AsNoTracking().FirstOrDefault(tg => tg.PlaylistId == playlistToAdd.PlaylistId && tg.TrackId == 2));
        }

        [TestMethod()]
        public void RemoveTracksFromPlaylist_ReturnsNotFound_Playlist()
        {
            // Arrange
            List<TrackDTO> listTrackToRemoveFromPlaylist = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1}
            };

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.RemoveTracksFromPlaylist(-1000, listTrackToRemoveFromPlaylist).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Playlist found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void RemoveTracksFromPlaylist_ReturnsNotFound_Track()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -9, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" };
            List<PlaylistTrack> listPlaylistTrackToAdd = new List<PlaylistTrack>() {
                new PlaylistTrack { PlaylistId = playlistToAdd.PlaylistId, TrackId = 1},
                new PlaylistTrack { PlaylistId = playlistToAdd.PlaylistId, TrackId = 2},
            };
            List<TrackDTO> listTrackToRemoveFromPlaylist = new List<TrackDTO>() {
                new TrackDTO { TrackId = 1000},
            };

            var test = _context.PlaylistTracks;

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            listPlaylistTrackToAdd.ForEach(playlistTrackToAdd => {
                _context.PlaylistTracks.Add(playlistTrackToAdd);
            });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (NotFoundObjectResult)_controller.AddTracksToPlaylist(playlistToAdd.PlaylistId, listTrackToRemoveFromPlaylist).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("At least one of the Track given wasn't found"));
        }

        [TestMethod()]
        public void DeletePlaylists_ReturnsOk()
        {
            // Arrange
            List<PlaylistDTO> listPlaylistToAdd = new List<PlaylistDTO>() {
                new PlaylistDTO(){ PlaylistId = -2001, KindId = 1, UserId = 1001, PlaylistName = "PlaylistOne" },
                new PlaylistDTO(){ PlaylistId = -2002, KindId = 1, UserId = 1001, PlaylistName = "PlaylistOne" },
                new PlaylistDTO(){ PlaylistId = -2003, KindId = 1, UserId = 1, PlaylistName = "PlaylistOne" },
                new PlaylistDTO(){ PlaylistId = -2004, KindId = 1, UserId = 1001, PlaylistName = "PlaylistOne" },
            };

            // Act
            listPlaylistToAdd.ForEach(pToAdd => {
                _context.Playlists.Add(_mapper.Map<Playlist>(pToAdd));
            });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.DeletePlaylists(1001).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Playlists.Where(p => p.UserId == 1001).ToList().Count, 0);
        }

        [TestMethod()]
        public void AddFavoritePlaylist_ReturnsOk()
        {
            // Act
            var actionToTest = (CreatedAtActionResult)_controller.AddFavoritePlaylist(1001).Result.Result;
            var resultToTest = (PlaylistDTO)actionToTest.Value;

            Console.WriteLine("a");

            // Assert
            Assert.AreEqual(_context.Playlists.Find(resultToTest.PlaylistId).PlaylistName, "Favorite Musics");   
            Assert.AreEqual(_context.Playlists.Find(resultToTest.PlaylistId).UserId, 1001);          
            Assert.AreEqual(_context.Playlists.Find(resultToTest.PlaylistId).KindId, 3);          
        }

        [TestMethod()]
        public void AddFavoritePlaylist_ReturnsAlreadyExists()
        {
            // Arrange
            PlaylistDTO playlistToAdd = new PlaylistDTO(){ PlaylistId = -1050, KindId = 3, UserId = 50, PlaylistName = "Favorite Musics" };

            // Act
            _context.Playlists.Add(_mapper.Map<Playlist>(playlistToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (BadRequestObjectResult)_controller.AddFavoritePlaylist(50).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format(string.Format("Favorite playlist already exists for User {0}", 50)));            
        }
    }
}
#pragma warning restore CS8602, CS8600, CS8604, CS8625
