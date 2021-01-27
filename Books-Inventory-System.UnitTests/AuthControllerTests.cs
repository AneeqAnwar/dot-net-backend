using Books_Inventory_System.Controllers;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.User;
using Books_Inventory_System.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    [TestFixture]
    public class AuthControllerTests
    {
        Mock<IAuthRepository> mockRepository;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IAuthRepository>();
        }

        [Test]
        public void Register_NewUser_ReturnsOk()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<int> { Data = 1 });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            var result = authController.Register(newUser);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Register_ExistingUser_ReturnsBadRequest()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<int> { Success = false });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            authController.Register(newUser);
            var result = authController.Register(newUser);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_ValidUser_ReturnsOk()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<int> { Data = 1 });
            mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<string> { Data = "some-token" });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            authController.Register(newUser);
            var result = authController.Login(GetUserLoginDto());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Login_InvalidUsername_ReturnsBadRequest()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<int> { Data = 1 });
            mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<string> { Success = false });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            authController.Register(newUser);

            UserLoginDto userLoginDto = GetUserLoginDto();
            userLoginDto.Username = "changed";

            var result = authController.Login(userLoginDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_InvalidPassword_ReturnsBadRequest()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<int> { Data = 1 });
            mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ServiceResponse<string> { Success = false });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

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
