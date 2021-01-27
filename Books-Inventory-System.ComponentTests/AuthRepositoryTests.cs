using System.Collections.Generic;
using Books_Inventory_System.Data;
using Books_Inventory_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Books_Inventory_System.ComponentTests
{
    [TestFixture]
    public class AuthRepositoryTests
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
        public void Register_NonExistingUser_ReturnsUserId()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            ServiceResponse<int> registerUserResponse = authRepository.Register(newUser, "123456");

            Assert.That(registerUserResponse, Is.InstanceOf<ServiceResponse<int>>());
            Assert.That(registerUserResponse.Success, Is.EqualTo(true));
            Assert.That(registerUserResponse.Data, Is.InstanceOf<int>());
        }

        [Test]
        public void Register_ExistingUser_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            authRepository.Register(newUser, "123456");

            ServiceResponse<int> registerUserResponse = authRepository.Register(newUser, "123456");

            Assert.That(registerUserResponse, Is.InstanceOf<ServiceResponse<int>>());
            Assert.That(registerUserResponse.Success, Is.EqualTo(false));
            Assert.That(registerUserResponse.Message, Is.EqualTo("User already exists."));
        }

        [Test]
        public void Login_ValidUser_ReturnsJwtToken()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            authRepository.Register(newUser, password);

            ServiceResponse<string> loginUserResponse = authRepository.Login(newUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(true));
            Assert.That(loginUserResponse.Data, Is.InstanceOf<string>());
        }

        [Test]
        public void Login_InvalidUsername_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            authRepository.Register(newUser, password);

            newUser.Username = "changed";
            ServiceResponse<string> loginUserResponse = authRepository.Login(newUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(false));
            Assert.That(loginUserResponse.Data, Is.EqualTo(null));
            Assert.That(loginUserResponse.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public void Login_InvalidPassword_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContext, configuration);
            authRepository.Register(newUser, password);

            password = "789";
            ServiceResponse<string> loginUserResponse = authRepository.Login(newUser.Username, password);

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
