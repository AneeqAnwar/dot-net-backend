using System.Collections.Generic;
using Books_Inventory_System.Data;
using Books_Inventory_System.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    [TestFixture]
    public class AuthRepositoryTests
    {
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IDataContext> dbContextMock;

        [SetUp]
        public void Setup()
        {
            List<User> users = new List<User>
            {
                GetDummyUser()
            };

            dbContextMock = new Mock<IDataContext>();
            dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<User>(users));
            dbContextMock.Setup(p => p.SaveChanges()).Returns(1);

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.Setup(a => a.Value).Returns("my test secret key");

            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection("AppSettings:Token")).Returns(mockConfSection.Object);
        }

        [Test]
        public void Register_NonExistingUser_ReturnsUserId()
        {
            User newUser = GetUser();
            AuthRepository authRepository = new AuthRepository(dbContextMock.Object, mockConfiguration.Object);
            ServiceResponse<int> registerUserResponse = authRepository.Register(newUser, "123456");

            Assert.That(registerUserResponse, Is.InstanceOf<ServiceResponse<int>>());
            Assert.That(registerUserResponse.Success, Is.EqualTo(true));
            Assert.That(registerUserResponse.Data, Is.InstanceOf<int>());
        }

        [Test]
        public void Register_ExistingUser_ReturnsError()
        {
            User newUser = GetUser();
            AuthRepository authRepository = new AuthRepository(dbContextMock.Object, mockConfiguration.Object);
            authRepository.Register(newUser, "123456");

            ServiceResponse<int> registerUserResponse = authRepository.Register(newUser, "123456");

            Assert.That(registerUserResponse, Is.InstanceOf<ServiceResponse<int>>());
            Assert.That(registerUserResponse.Success, Is.EqualTo(false));
            Assert.That(registerUserResponse.Message, Is.EqualTo("User already exists."));
        }

        [Test]
        public void Login_ValidUser_ReturnsJwtToken()
        {
            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContextMock.Object, mockConfiguration.Object);
            authRepository.Register(newUser, password);

            ServiceResponse<string> loginUserResponse = authRepository.Login(newUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(true));
            Assert.That(loginUserResponse.Data, Is.InstanceOf<string>());
        }

        [Test]
        public void Login_InvalidUsername_ReturnsError()
        {
            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContextMock.Object, mockConfiguration.Object);
            authRepository.Register(newUser, password);

            User loginUser = GetUser();
            loginUser.Username = "changed";
            ServiceResponse<string> loginUserResponse = authRepository.Login(loginUser.Username, password);

            Assert.That(loginUserResponse, Is.InstanceOf<ServiceResponse<string>>());
            Assert.That(loginUserResponse.Success, Is.EqualTo(false));
            Assert.That(loginUserResponse.Data, Is.EqualTo(null));
            Assert.That(loginUserResponse.Message, Is.EqualTo("User not found"));
        }

        [Test]
        public void Login_InvalidPassword_ReturnsError()
        {
            User newUser = GetUser();
            string password = "123456";
            AuthRepository authRepository = new AuthRepository(dbContextMock.Object, mockConfiguration.Object);
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

        private User GetDummyUser()
        {
            return new User
            {
                Id = 99,
                Username = "Dummy User",
            };
        }
    }
}
