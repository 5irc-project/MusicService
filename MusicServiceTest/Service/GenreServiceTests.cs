using MusicService.Models;
using MusicService.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using MusicService.Services.Implementations;
using AutoMapper;
using MusicService.Helpers;
using MusicService.DTOs;

namespace MusicServiceTest.Service
{
    [TestClass()]
    public class GenreServiceTests
    {
        private MusicServiceDBContext _context;
        private IGenreService? _service;
        private IMapper _mapper;
        
        public GenreServiceTests(){
            var dbContextOptions = new DbContextOptionsBuilder<MusicServiceDBContext>().UseInMemoryDatabase("MusicServiceDBTest");
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfiles()));

            _context = new MusicServiceDBContext(dbContextOptions.Options);
            _context.Database.EnsureCreated();
            _mapper = mappingConfig.CreateMapper();            
        }

        [TestInitialize]
        public void Setup(){
            _service = new GenreService(_context, _mapper);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _service = null;
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
            #pragma warning disable CS8602
            listGenreToAdd.ForEach(genreToAdd => {
                _context.Genres.Add(_mapper.Map<Genre>(genreToAdd));
            });
            _context.SaveChanges();
            List<GenreDTO> listGenreToTest = _service.GetGenres().Result.ToList();
            #pragma warning restore CS8602

            // Assert
            CollectionAssert.AllItemsAreNotNull(listGenreToTest);
            Assert.AreEqual(listGenreToTest.SequenceEqual(listGenreToAdd), true);
        }   
    }
}