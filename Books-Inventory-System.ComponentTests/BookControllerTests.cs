using AutoMapper;
using Books_Inventory_System.Controllers;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Services.BookService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Books_Inventory_System.ComponentTests
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mapper mapper;
        DataContext dbContext;

        [SetUp]
        public void Setup()
        {
            AutoMapperProfile myProfile = new AutoMapperProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(configuration);

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "BooksDatabase")
            .Options;
            dbContext = new DataContext(options);
        }

        [Test]
        public void AddBook_NewBook_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            var result = bookController.AddBook(newBook);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetBookById_ExistingBook_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            bookController.AddBook(newBook);

            var result = bookController.GetSingleBook(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void GetAllBooks_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            bookController.AddBook(newBook);

            var result = bookController.Get();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void UpdateBook_ExistingBook_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            bookController.AddBook(newBook);

            UpdateBookDto updatedBook = GetUpdateBookDto();
            var result = bookController.UpdateBook(updatedBook);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void UpdateBook_NonExistingBook_ReturnsNotFound()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            UpdateBookDto updatedBook = GetUpdateBookDto();
            var result = bookController.UpdateBook(updatedBook);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void DeleteBook_ExistingBook_ReturnsOk()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            bookController.AddBook(newBook);

            var result = bookController.DeleteBook(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void DeleteBook_NonExistingBook_ReturnsNotFound()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            var result = bookController.DeleteBook(1);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public void CheckStatus_ReturnsOk()
        {
            BookService bookService = new BookService(mapper, dbContext);
            BookController bookController = new BookController(bookService);

            var result = bookController.CheckStatus();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        private AddBookDto GetAddBookDto()
        {
            return new AddBookDto
            {
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 899,
                CategoryId = 1
            };
        }

        private UpdateBookDto GetUpdateBookDto()
        {
            return new UpdateBookDto
            {
                Id = 1,
                Name = "The 7 habits of highly effective people",
                Author = "Stephen R. Covey"
            };
        }
    }
}
