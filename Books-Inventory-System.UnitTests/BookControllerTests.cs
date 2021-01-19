using System.Threading.Tasks;
using Books_Inventory_System.Controllers;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Services.BookService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    [TestFixture]
    public class BookControllerTests
    {
        Mock<IBookService> mockService;

        [SetUp]
        public void Setup()
        {
            mockService = new Mock<IBookService>();
        }

        [Test]
        public async Task AddBook_NewBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .ReturnsAsync(BookTestData.AddBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            var result = await bookController.AddBook(newBook);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetBookById_ExistingBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .ReturnsAsync(BookTestData.AddBookServiceResponse());
            mockService.Setup(s => s.GetBookById(It.IsAny<int>()))
                .ReturnsAsync(BookTestData.GetSingleBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            await bookController.AddBook(newBook);

            var result = await bookController.GetSingleBook(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllBooks_ReturnsOk()
        {
            mockService.Setup(s => s.GetAllBooks())
                .ReturnsAsync(BookTestData.GetAllBooksServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            await bookController.AddBook(newBook);

            var result = await bookController.Get();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateBook_ExistingBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .ReturnsAsync(BookTestData.AddBookServiceResponse());
            mockService.Setup(s => s.UpdateBook(It.IsAny<UpdateBookDto>()))
                .ReturnsAsync(BookTestData.UpdateBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            await bookController.AddBook(newBook);

            UpdateBookDto updatedBook = BookTestData.UpdateBookDto();
            var result = await bookController.UpdateBook(updatedBook);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateBook_NonExistingBook_ReturnsNotFound()
        {
            mockService.Setup(s => s.UpdateBook(It.IsAny<UpdateBookDto>()))
                .ReturnsAsync(BookTestData.UpdateBookServiceResponseNullData());

            BookController bookController = new BookController(mockService.Object);

            UpdateBookDto updatedBook = BookTestData.UpdateBookDto();
            var result = await bookController.UpdateBook(updatedBook);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteBook_ExistingBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .ReturnsAsync(BookTestData.AddBookServiceResponse());
            mockService.Setup(s => s.DeleteBook(It.IsAny<int>()))
                .ReturnsAsync(BookTestData.DeleteBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            await bookController.AddBook(newBook);

            var result = await bookController.DeleteBook(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteBook_NonExistingBook_ReturnsNotFound()
        {
            mockService.Setup(s => s.DeleteBook(It.IsAny<int>()))
                .ReturnsAsync(BookTestData.DeleteBookServiceResponseNullData());

            BookController bookController = new BookController(mockService.Object);

            var result = await bookController.DeleteBook(1);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void CheckStatus_ReturnsOk()
        {
            BookController bookController = new BookController(mockService.Object);

            var result = bookController.CheckStatus();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
