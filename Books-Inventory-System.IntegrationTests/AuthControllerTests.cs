using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Books_Inventory_System.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Books_Inventory_System.IntegrationTests
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly string RegisterEndpoint = "/auth/register";
        private readonly string LoginEndpoint = "/auth/login";
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AuthControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            var request = new
            {
                Url = RegisterEndpoint,
                Body = new
                {
                    Username = "User1",
                    Password = "123456"
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PostAsync(request.Url, body);

            var responseString = await response.Content.ReadAsStringAsync();
            var registerUserResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            registerUserResponse.Data.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Register_ExistingUser_ReturnsBadRequest()
        {
            var request = new
            {
                Url = RegisterEndpoint,
                Body = new
                {
                    Username = "User2",
                    Password = "123456"
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            await _client.PostAsync(request.Url, body);

            var response = await _client.PostAsync(request.Url, body);

            var responseString = await response.Content.ReadAsStringAsync();
            var registerUserResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            registerUserResponse.Data.Should().Be(0);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsOk()
        {
            var registerUserRequest = new
            {
                Url = RegisterEndpoint,
                Body = new
                {
                    Username = "User3",
                    Password = "123456"
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(registerUserRequest.Body), Encoding.Default, "application/json");
            await _client.PostAsync(registerUserRequest.Url, body);

            var loginUserRequest = new
            {
                Url = LoginEndpoint,
                Body = registerUserRequest.Body
            };

            var loginRequestBody = new StringContent(JsonConvert.SerializeObject(loginUserRequest.Body), Encoding.Default, "application/json");
            var response = await _client.PostAsync(loginUserRequest.Url, loginRequestBody);

            var responseString = await response.Content.ReadAsStringAsync();
            var loginUserResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            loginUserResponse.Data.Should().BeOfType<string>();
        }

        [Fact]
        public async Task Login_InvalidUsername_ReturnsBadRequest()
        {
            var loginUserRequest = new
            {
                Url = LoginEndpoint,
                Body = new
                {
                    Username = "User4",
                    Password = "123456"
                }
            };

            var loginRequestBody = new StringContent(JsonConvert.SerializeObject(loginUserRequest.Body), Encoding.Default, "application/json");
            var response = await _client.PostAsync(loginUserRequest.Url, loginRequestBody);

            var responseString = await response.Content.ReadAsStringAsync();
            var loginUserResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            loginUserResponse.Data.Should().BeNull();
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsBadRequest()
        {
            var registerUserRequest = new
            {
                Url = RegisterEndpoint,
                Body = new
                {
                    Username = "User3",
                    Password = "123456"
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(registerUserRequest.Body), Encoding.Default, "application/json");
            await _client.PostAsync(registerUserRequest.Url, body);

            var loginUserRequest = new
            {
                Url = LoginEndpoint,
                Body = new
                {
                    Username = "User3",
                    Password = "Invalid"
                }
            };

            var loginRequestBody = new StringContent(JsonConvert.SerializeObject(loginUserRequest.Body), Encoding.Default, "application/json");
            var response = await _client.PostAsync(loginUserRequest.Url, loginRequestBody);

            var responseString = await response.Content.ReadAsStringAsync();
            var loginUserResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            loginUserResponse.Data.Should().BeNull();
        }
    }
}
