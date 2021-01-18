﻿using System.Threading.Tasks;
using Books_Inventory_System.Data;
using Books_Inventory_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    [TestFixture]
    public class AuthRepositoryTests
    {
        DataContext dbContext;
        Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "UsersDatabase")
            .Options;
            dbContext = new DataContext(options);

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.Setup(a => a.Value).Returns("my test secret key");

            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection("AppSettings:Token")).Returns(mockConfSection.Object);
        }

        [Test]
        public async Task Register_NonExistingUser_ReturnsUserId()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            AuthRepository authRepository = new AuthRepository(dbContext, mockConfiguration.Object);
            ServiceResponse<int> registerUserResponse = await authRepository.Register(newUser, "123456");

            Assert.That(registerUserResponse, Is.InstanceOf<ServiceResponse<int>>());
            Assert.That(registerUserResponse.Success, Is.EqualTo(true));
            Assert.That(registerUserResponse.Data, Is.InstanceOf<int>());
        }

        [Test]
        public async Task Register_ExistingUser_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            AuthRepository authRepository = new AuthRepository(dbContext, mockConfiguration.Object);
            await authRepository.Register(newUser, "123456");

            ServiceResponse<int> registerUserResponse = await authRepository.Register(newUser, "123456");

            Assert.That(registerUserResponse, Is.InstanceOf<ServiceResponse<int>>());
            Assert.That(registerUserResponse.Success, Is.EqualTo(false));
            Assert.That(registerUserResponse.Message, Is.EqualTo("User already exists."));
        }

        [Test]
        public async Task Login_ValidUser_ReturnsJwtToken()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContext, mockConfiguration.Object);
            await authRepository.Register(newUser, password);

            ServiceResponse<string> loginUserResponse = await authRepository.Login(newUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(true));
            Assert.That(loginUserResponse.Data, Is.InstanceOf<string>());
        }

        [Test]
        public async Task Login_InvalidUsername_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContext, mockConfiguration.Object);
            await authRepository.Register(newUser, password);

            newUser.Username = "changed";
            ServiceResponse<string> loginUserResponse = await authRepository.Login(newUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(false));
            Assert.That(loginUserResponse.Data, Is.EqualTo(null));
            Assert.That(loginUserResponse.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public async Task Login_InvalidPassword_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContext, mockConfiguration.Object);
            await authRepository.Register(newUser, password);

            password = "789";
            ServiceResponse<string> loginUserResponse = await authRepository.Login(newUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(false));
            Assert.That(loginUserResponse.Data, Is.EqualTo(null));
            Assert.That(loginUserResponse.Message, Is.EqualTo("Incorrect password"));
        }

        private User GetUser()
        {
            return new User
            {
                Id = 1,
                Username = "Aneeq",
            };
        }
    }
}
