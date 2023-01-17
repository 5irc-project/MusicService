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

#pragma warning disable CS8602, CS8600, CS8604
namespace MusicServiceTest.Service
{
    [TestClass()]
    public class KindControllerTests
    {
        private KindController? _controller;
        private MusicServiceDBContext _context;
        private IKindService _service;
        private IMapper _mapper;
        
        public KindControllerTests(){
            var dbContextOptions = new DbContextOptionsBuilder<MusicServiceDBContext>().UseInMemoryDatabase("MusicServiceDBTest");
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfiles()));

            _context = new MusicServiceDBContext(dbContextOptions.Options);
            _context.Database.EnsureCreated();
            _mapper = mappingConfig.CreateMapper();  
            _service = new KindService(_context, _mapper);
        }

        [TestInitialize]
        public void Setup(){
            _controller = new KindController(_service);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _controller = null;
        }

        [TestMethod()]
        public void GetKinds_ReturnsOk()
        {
            // Arrange
            List<KindDTO> listKindToAdd = new List<KindDTO>() {
                new KindDTO { KindId = 1, Name = "KindUn" },
                new KindDTO { KindId = 2, Name = "KindDeux" },
            };

            // Act
            listKindToAdd.ForEach(kindToAdd => {
                _context.Kinds.Add(_mapper.Map<Kind>(kindToAdd));
            });
            _context.SaveChanges();
            List<KindDTO> listKindToTest = _controller.GetKinds().Result.Value;

            // Assert
            CollectionAssert.AllItemsAreNotNull(listKindToTest);
            Assert.AreEqual(listKindToTest.SequenceEqual(listKindToAdd), true);
        }   

        [TestMethod()]
        public void GetKind_ReturnsOk()
        {
            // Arrange
            KindDTO kindToAdd = new KindDTO(){
                KindId = -1,
                Name = "KindTestGetKind"
            };

            // Act
            _context.Kinds.Add(_mapper.Map<Kind>(kindToAdd));
            _context.SaveChanges();
            KindDTO kindToTest = _controller.GetKind(kindToAdd.KindId).Result.Value;

            // Assert
            Assert.IsInstanceOfType(kindToTest, typeof(KindDTO));
            Assert.AreEqual(kindToTest, kindToAdd);
        }
        
        [TestMethod()]
        public void GetKind_ReturnsNotFound()
        {
            // Act
            var actionToTest = (NotFoundObjectResult)_controller.GetKind(-1000).Result.Result;

            // Assert
            Assert.AreEqual(actionToTest.Value, string.Format("No Kind found with ID = {0}", -1000));
        }

        // [TestMethod()]
        // public void PutKind_ReturnsOk()
        // {
        //     // Arrange
        //     KindDTO kindToAdd = new KindDTO(){
        //         KindId = -2,
        //         Name = "KindTestPutKind"
        //     };
        //     KindDTO kindToPut = new KindDTO(){
        //         KindId = -2,
        //         Name = "KindTestPutKindAfterPut"
        //     };

        //     // Act
        //     _context.Kinds.Add(_mapper.Map<Kind>(kindToAdd));
        //     _context.SaveChanges();
        //     _context.ChangeTracker.Clear();
        //     var actionToTest = _controller.PutKind(kindToPut.KindId, kindToPut).Result;

        //     // Assert
        //     Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
        //     Assert.AreEqual(_context.Kinds.Find(kindToPut.KindId).Name, kindToPut.Name);         
        // }

        // [TestMethod()]
        // public void PutKind_ReturnsNotFound()
        // {
        //     // Arrange
        //     KindDTO kindToPut = new KindDTO(){
        //         KindId = -1000,
        //         Name = "KindTestPutKindAfterPut"
        //     };

        //     // Act
        //     var actionToTest = (NotFoundObjectResult)_controller.PutKind(kindToPut.KindId, kindToPut).Result;

        //     // Assert
        //     Assert.AreEqual(actionToTest.Value, string.Format("No Kind found with ID = {0}", kindToPut.KindId));         
        // }

        // [TestMethod()]
        // public void PutKind_ReturnsBadRequest()
        // {
        //     // Arrange
        //     KindDTO kindToPut = new KindDTO(){
        //         KindId = -1000,
        //         Name = "KindTestPutKindAfterPut"
        //     };

        //     // Act
        //     var actionToTest = _controller.PutKind(-1001, kindToPut).Result;

        //     // Assert
        //     Assert.IsInstanceOfType(actionToTest, typeof(BadRequestResult));         
        // }

        // [TestMethod()]
        // public void PostKind_ReturnsOk()
        // {
        //     // Arrange
        //     KindDTO kindToPost = new KindDTO(){
        //         KindId = -3,
        //         Name = "KindTestPost"
        //     };

        //     // Act
        //     var actionToTest = _controller.PostKind(kindToPost).Result.Result;

        //     // Assert
        //     Assert.IsInstanceOfType(actionToTest, typeof(CreatedAtActionResult));
        //     Assert.AreEqual(_mapper.Map<KindDTO>(_context.Kinds.Find(kindToPost.KindId)), kindToPost);        
        // }

        // [TestMethod()]
        // public void PostKind_ReturnsAlreadyExists_Id()
        // {
        //     // Arrange
        //     KindDTO kindToAdd = new KindDTO(){
        //         KindId = -4,
        //         Name = "KindTestAlreadyExists"
        //     };
        //     KindDTO kindToPost = new KindDTO(){
        //         KindId = -4,
        //         Name = "Nothing"
        //     };

        //     // Act
        //     _context.Kinds.Add(_mapper.Map<Kind>(kindToAdd));
        //     _context.SaveChanges();
        //     _context.ChangeTracker.Clear();
        //     var actionToTest = (BadRequestObjectResult)_controller.PostKind(kindToPost).Result.Result;
        //     // Assert
        //     Assert.AreEqual(actionToTest.Value, string.Format("Kind with ID = {0} already exists", kindToPost.KindId));        
        // }

        // [TestMethod()]
        // public void PostKind_ReturnsAlreadyExists_Name()
        // {
        //     // Arrange
        //     KindDTO kindToAdd = new KindDTO(){
        //         KindId = -5,
        //         Name = "KindTestAlreadyExists"
        //     };
        //     KindDTO kindToPost = new KindDTO(){
        //         KindId = -1000,
        //         Name = "KindTestAlreadyExists"
        //     };

        //     // Act
        //     _context.Kinds.Add(_mapper.Map<Kind>(kindToAdd));
        //     _context.SaveChanges();
        //     _context.ChangeTracker.Clear();
        //     var actionToTest = (BadRequestObjectResult)_controller.PostKind(kindToPost).Result.Result;
        //     // Assert
        //     Assert.AreEqual(actionToTest.Value, string.Format("Kind with Name {0} already exists", kindToPost.Name));        
        // }

        // [TestMethod()]
        // public void DeleteKind_ReturnsOk()
        // {
        //     // Arrange
        //     KindDTO kindToAdd = new KindDTO(){
        //         KindId = -6,
        //         Name = "KindTestDelete"
        //     };

        //     // Act
        //     _context.Kinds.Add(_mapper.Map<Kind>(kindToAdd));
        //     _context.SaveChanges();
        //     _context.ChangeTracker.Clear();
        //     var actionToTest = _controller.DeleteKind(kindToAdd.KindId).Result;
        //     // Assert
        //     Assert.IsInstanceOfType(actionToTest, typeof(NoContentResult));
        //     Assert.AreEqual(_context.Kinds.Find(kindToAdd.KindId), null);  
        // }

        // [TestMethod()]
        // public void DeleteKind_ReturnsNotFound()
        // {
        //     var actionToTest = (NotFoundObjectResult)_controller.DeleteKind(-1000).Result;
        //     // Assert
        //     Assert.AreEqual(actionToTest.Value, string.Format("No Kind found with ID = {0}", -1000));  
        // }
    }
}
#pragma warning restore CS8602, CS8600, CS8604
