using System.Collections.Generic;
using Books_Inventory_System.Controllers;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Books_Inventory_System.ComponentTests
{
    [TestFixture]
    public class AuthControllerTests
    {
        DataContext dbContext;
        IConfigurationRoot configuration;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "UsersDatabase")
            .Options;
            dbContext = new DataContext(options);

            var myConfiguration = new Dictionary<string, string>
            {
                {"AppSettings:Token", "my test secret key"}
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        [Test]
        public void Register_NewUser_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UserRegisterDto newUser = GetUserRegisterDto();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            AuthController authController = new AuthController(authRepository);

            var result = authController.Register(newUser);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Register_ExistingUser_ReturnsBadRequest()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UserRegisterDto newUser = GetUserRegisterDto();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            AuthController authController = new AuthController(authRepository);

            authController.Register(newUser);
            var result = authController.Register(newUser);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_ValidUser_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UserRegisterDto newUser = GetUserRegisterDto();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            AuthController authController = new AuthController(authRepository);

            authController.Register(newUser);
            var result = authController.Login(GetUserLoginDto());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Login_InvalidUsername_ReturnsBadRequest()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UserRegisterDto newUser = GetUserRegisterDto();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            AuthController authController = new AuthController(authRepository);

            authController.Register(newUser);

            UserLoginDto userLoginDto = GetUserLoginDto();
            userLoginDto.Username = "changed";

            var result = authController.Login(userLoginDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_InvalidPassword_ReturnsBadRequest()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UserRegisterDto newUser = GetUserRegisterDto();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            AuthController authController = new AuthController(authRepository);

            authController.Register(newUser);

            UserLoginDto userLoginDto = GetUserLoginDto();
            userLoginDto.Password = "changed";

            var result = authController.Login(userLoginDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        private UserRegisterDto GetUserRegisterDto()
        {
            return new UserRegisterDto
            {
                Username = "Aneeq",
                Password = "123456"
            };
        }

        private UserLoginDto GetUserLoginDto()
        {
            return new UserLoginDto
            {
                Username = "Aneeq",
                Password = "123456"
            };
        }
    }
}
