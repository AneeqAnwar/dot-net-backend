using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Books_Inventory_System.IntegrationTests
{
    public class BookControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public BookControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllBooks_WithoutAnyBooks_ReturnsEmptyList()
        {
            var response = await _client.GetAsync("/book");

            var responseString = await response.Content.ReadAsStringAsync();
            var getAllBooksResponse = JsonConvert.DeserializeObject<ServiceResponse<List<GetBookDto>>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            getAllBooksResponse.Data.Should().BeOfType<List<GetBookDto>>();
        }

        [Fact]
        public async Task AddBook_WithValidInput_ReturnsBooksList()
        {
            var request = new
            {
                Url = "/book",
                Body = new
                {
                    Name = "Delivering Happiness"
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PostAsync(request.Url, body);

            var responseString = await response.Content.ReadAsStringAsync();
            var addBookResponse = JsonConvert.DeserializeObject<ServiceResponse<List<GetBookDto>>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            addBookResponse.Data.Should().BeOfType<List<GetBookDto>>();
        }

        [Fact]
        public async Task GetSingleBook_WithValidInput_ReturnsBook()
        {
            GetBookDto addedBook = await this.AddBook("Delivering Happiness");

            var response = await _client.GetAsync("/book/" + addedBook.Id);
            var responseString = await response.Content.ReadAsStringAsync();

            var getBookResponse = JsonConvert.DeserializeObject<ServiceResponse<GetBookDto>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            getBookResponse.Data.Should().BeOfType<GetBookDto>();
        }

        [Fact]
        public async Task UpdateBook_WithValidInput_ReturnsBook()
        {
            GetBookDto addedBook = await this.AddBook("Delivering Happiness");

            var request = new
            {
                Url = "/book",
                Body = new
                {
                    Id = addedBook.Id,
                    Name = "Updated Name"
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PutAsync(request.Url, body);

            var responseString = await response.Content.ReadAsStringAsync();
            var updateBookResponse = JsonConvert.DeserializeObject<ServiceResponse<GetBookDto>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updateBookResponse.Data.Should().BeOfType<GetBookDto>();
        }

        [Fact]
        public async Task DeleteBook_WithValidInput_ReturnsBook()
        {
            string bookName = "Delivering Happiness";
            GetBookDto addedBook = await this.AddBook(bookName);

            var response = await _client.DeleteAsync("/book/" + addedBook.Id);

            var responseString = await response.Content.ReadAsStringAsync();
            var deleteBookResponse = JsonConvert.DeserializeObject<ServiceResponse<List<GetBookDto>>>(responseString);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteBookResponse.Data.Should().BeOfType<List<GetBookDto>>();
        }

        [Fact]
        public async Task CheckStatus_WithValidInput_ReturnsOk()
        {
            var response = await _client.GetAsync("/book/Status");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<GetBookDto> AddBook(string bookName)
        {
            var request = new
            {
                Url = "/book",
                Body = new
                {
                    Name = bookName
                }
            };

            var body = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");
            var response = await _client.PostAsync(request.Url, body);

            var responseString = await response.Content.ReadAsStringAsync();
            var addBookResponse = JsonConvert.DeserializeObject<ServiceResponse<List<GetBookDto>>>(responseString);

            return addBookResponse.Data[0];
        }
    }
}
