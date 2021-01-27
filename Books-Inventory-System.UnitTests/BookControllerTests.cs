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
        public void AddBook_NewBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .Returns(BookTestData.AddBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            var result = bookController.AddBook(newBook);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetBookById_ExistingBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .Returns(BookTestData.AddBookServiceResponse());
            mockService.Setup(s => s.GetBookById(It.IsAny<int>()))
                .Returns(BookTestData.GetSingleBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            bookController.AddBook(newBook);

            var result = bookController.GetSingleBook(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetAllBooks_ReturnsOk()
        {
            mockService.Setup(s => s.GetAllBooks())
                .Returns(BookTestData.GetAllBooksServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            bookController.AddBook(newBook);

            var result = bookController.Get();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void UpdateBook_ExistingBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .Returns(BookTestData.AddBookServiceResponse());
            mockService.Setup(s => s.UpdateBook(It.IsAny<UpdateBookDto>()))
                .Returns(BookTestData.UpdateBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            bookController.AddBook(newBook);

            UpdateBookDto updatedBook = BookTestData.UpdateBookDto();
            var result = bookController.UpdateBook(updatedBook);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void UpdateBook_NonExistingBook_ReturnsNotFound()
        {
            mockService.Setup(s => s.UpdateBook(It.IsAny<UpdateBookDto>()))
                .Returns(BookTestData.UpdateBookServiceResponseNullData());

            BookController bookController = new BookController(mockService.Object);

            UpdateBookDto updatedBook = BookTestData.UpdateBookDto();
            var result = bookController.UpdateBook(updatedBook);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void DeleteBook_ExistingBook_ReturnsOk()
        {
            mockService.Setup(s => s.AddBook(It.IsAny<AddBookDto>()))
                .Returns(BookTestData.AddBookServiceResponse());
            mockService.Setup(s => s.DeleteBook(It.IsAny<int>()))
                .Returns(BookTestData.DeleteBookServiceResponse());

            BookController bookController = new BookController(mockService.Object);
            AddBookDto newBook = BookTestData.AddBookDto();

            bookController.AddBook(newBook);

            var result = bookController.DeleteBook(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void DeleteBook_NonExistingBook_ReturnsNotFound()
        {
            mockService.Setup(s => s.DeleteBook(It.IsAny<int>()))
                .Returns(BookTestData.DeleteBookServiceResponseNullData());

            BookController bookController = new BookController(mockService.Object);

            var result = bookController.DeleteBook(1);

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
