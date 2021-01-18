﻿using System.Threading.Tasks;
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
        public async Task Register_NewUser_ReturnsOk()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<int> { Data = 1 });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            var result = await authController.Register(newUser);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task Register_ExistingUser_ReturnsBadRequest()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<int> { Success = false });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            await authController.Register(newUser);
            var result = await authController.Register(newUser);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Login_ValidUser_ReturnsOk()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<int> { Data = 1 });
            mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<string> { Data = "some-token" });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            await authController.Register(newUser);
            var result = await authController.Login(GetUserLoginDto());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task Login_InvalidUsername_ReturnsBadRequest()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<int> { Data = 1 });
            mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<string> { Success = false });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            await authController.Register(newUser);

            UserLoginDto userLoginDto = GetUserLoginDto();
            userLoginDto.Username = "changed";

            var result = await authController.Login(userLoginDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Login_InvalidPassword_ReturnsBadRequest()
        {
            mockRepository.Setup(r => r.Register(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<int> { Data = 1 });
            mockRepository.Setup(r => r.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ServiceResponse<string> { Success = false });

            AuthController authController = new AuthController(mockRepository.Object);
            UserRegisterDto newUser = GetUserRegisterDto();

            await authController.Register(newUser);

            UserLoginDto userLoginDto = GetUserLoginDto();
            userLoginDto.Password = "changed";

            var result = await authController.Login(userLoginDto);

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
