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

namespace MusicServiceTest.Service
{
    [TestClass()]
    public class GenreControllerTests
    {
        private GenreController? _controller;
        private MusicServiceDBContext _context;
        private IGenreService _service;
        private IMapper _mapper;
        
        public GenreControllerTests(){
            var dbContextOptions = new DbContextOptionsBuilder<MusicServiceDBContext>().UseInMemoryDatabase("MusicServiceDBTest");
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfiles()));

            _context = new MusicServiceDBContext(dbContextOptions.Options);
            _context.Database.EnsureCreated();
            _mapper = mappingConfig.CreateMapper();  
            _service = new GenreService(_context, _mapper);
        }

        [TestInitialize]
        public void Setup(){
            _controller = new GenreController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _controller = null;
        }

        [TestMethod()]
        public void GetGenres_ReturnsOk()
        {
            // Arrange
            List<GenreDTO> listGenreToAdd = new List<GenreDTO>() {
                new GenreDTO { GenreId = 1, Name = "GenreUn" },
                new GenreDTO { GenreId = 2, Name = "GenreDeux" },
            };

            // Act
            listGenreToAdd.ForEach(genreToAdd => {
                _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            });
            _context.SaveChanges();
            List<GenreDTO> listGenreToTest = _controller.GetGenres().Result.Value;

            // Assert
            CollectionAssert.AllItemsAreNotNull(listGenreToTest);
            Assert.AreEqual(listGenreToTest.SequenceEqual(listGenreToAdd), true);
        }   

        [TestMethod()]
        public void GetGenre_ReturnsOk()
        {
            // Arrange
            GenreDTO genreToAdd = new GenreDTO(){
                GenreId = -1,
                Name = "GenreTestGetGenre"
            };

            // Act
            _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            _context.SaveChanges();
            GenreDTO genreToTest = _controller.GetGenre(genreToAdd.GenreId).Result.Value;

            // Assert
            Assert.IsInstanceOfType(genreToTest, typeof(GenreDTO));
            Assert.AreEqual(genreToTest, genreToAdd);
        }
        
        [TestMethod()]
        public void GetGenre_ReturnsNotFound()
        {
            // Act
            var actionToTest = (NotFoundObjectResult)_controller.GetGenre(-1000).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Genre found with ID = {0}", -1000));
        }

        [TestMethod()]
        public void PutGenre_ReturnsOk()
        {
            // Arrange
            GenreDTO genreToAdd = new GenreDTO(){
                GenreId = -2,
                Name = "GenreTestPutGenre"
            };
            GenreDTO genreToPut = new GenreDTO(){
                GenreId = -2,
                Name = "GenreTestPutGenreAfterPut"
            };

            // Act
            _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.PutGenre(genreToPut.GenreId, genreToPut).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Genres.Find(genreToAdd.GenreId).Name, genreToPut.Name);
        }

        [TestMethod()]
        public void PutGenre_ReturnsNotFound()
        {
            // Arrange
            GenreDTO genreToPut = new GenreDTO(){
                GenreId = -1000,
                Name = "GenreTestPutGenreAfterPut"
            };

            // Act
            var actionToTest = (NotFoundObjectResult)_controller.PutGenre(genreToPut.GenreId, genreToPut).Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Genre found with ID = {0}", genreToPut.GenreId));         
        }

        [TestMethod()]
        public void PutGenre_ReturnsBadRequest()
        {
            // Arrange
            GenreDTO genreToPut = new GenreDTO(){
                GenreId = -1000,
                Name = "GenreTestPutGenreAfterPut"
            };

            // Act
            var actionToTest = _controller.PutGenre(-1001, genreToPut).Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(BadRequestResult));         
        }

        [TestMethod()]
        public void PostGenre_ReturnsOk()
        {
            // Arrange
            GenreDTO genreToPost = new GenreDTO(){
                GenreId = -3,
                Name = "GenreTestPost"
            };

            // Act
            var actionToTest = _controller.PostGenre(genreToPost).Result.Result;

            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(CreatedAtActionResult));
            Assert.AreEqual(_mapper.Map<GenreDTO>(_context.Genres.Find(genreToPost.GenreId)), genreToPost);        
        }

        [TestMethod()]
        public void PostGenre_ReturnsAlreadyExists_Id()
        {
            // Arrange
            GenreDTO genreToAdd = new GenreDTO(){
                GenreId = -4,
                Name = "GenreTestAlreadyExists"
            };
            GenreDTO genreToPost = new GenreDTO(){
                GenreId = -4,
                Name = "Nothing"
            };

            // Act
            _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (BadRequestObjectResult)_controller.PostGenre(genreToPost).Result.Result;
            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("Genre with ID = {0} already exists", genreToPost.GenreId));        
        }

        [TestMethod()]
        public void PostGenre_ReturnsAlreadyExists_Name()
        {
            // Arrange
            GenreDTO genreToAdd = new GenreDTO(){
                GenreId = -5,
                Name = "GenreTestAlreadyExists"
            };
            GenreDTO genreToPost = new GenreDTO(){
                GenreId = -1000,
                Name = "GenreTestAlreadyExists"
            };

            // Act
            _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = (BadRequestObjectResult)_controller.PostGenre(genreToPost).Result.Result;
            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("Genre with Name {0} already exists", genreToPost.Name));        
        }

        [TestMethod()]
        public void DeleteGenre_ReturnsOk()
        {
            // Arrange
            GenreDTO genreToAdd = new GenreDTO(){
                GenreId = -6,
                Name = "GenreTestDelete"
            };

            // Act
            _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var actionToTest = _controller.DeleteGenre(genreToAdd.GenreId).Result;
            // Assert
            Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
            Assert.AreEqual(_context.Genres.Find(genreToAdd.GenreId), null);  
        }

        [TestMethod()]
        public void DeleteGenre_ReturnsNotFound()
        {
            var actionToTest = (NotFoundObjectResult)_controller.DeleteGenre(-1000).Result;
            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Genre found with ID = {0}", -1000));  
        }
    }
}