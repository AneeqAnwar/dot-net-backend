using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using Books_Inventory_System.Services.BookService;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Books_Inventory_System.ComponentTests
{
    [TestFixture]
    public class BookServiceTests
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
        public void AddBook_NewBook_ReturnsGetBookDtoList()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = bookService.AddBook(newBook);
            GetBookDto savedBook = addBookResponse.Data.First();

            Assert.That(addBookResponse.Success, Is.EqualTo(true));
            Assert.That(addBookResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(addBookResponse.Data, Is.InstanceOf<List<GetBookDto>>());
            Assert.That(addBookResponse.Data.Count, Is.EqualTo(1));
            Assert.That(savedBook, Is.InstanceOf<GetBookDto>());
            Assert.That(savedBook.Name, Is.EqualTo(newBook.Name));
            Assert.That(savedBook.Author, Is.EqualTo(newBook.Author));
            Assert.That(savedBook.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetBookById_BookId_ReturnsGetBookDto()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = bookService.AddBook(newBook);
            GetBookDto savedBook = addBookResponse.Data.First();

            ServiceResponse<GetBookDto> getBookResponse = bookService.GetBookById(savedBook.Id);
            GetBookDto receivedBook = getBookResponse.Data;

            Assert.That(getBookResponse.Success, Is.EqualTo(true));
            Assert.That(getBookResponse, Is.InstanceOf<ServiceResponse<GetBookDto>>());
            Assert.That(getBookResponse.Data, Is.InstanceOf<GetBookDto>());
            Assert.That(receivedBook, Is.InstanceOf<GetBookDto>());
            Assert.That(receivedBook.Name, Is.EqualTo(savedBook.Name));
            Assert.That(receivedBook.Author, Is.EqualTo(savedBook.Author));
        }

        [Test]
        public void GetAllBooks_GetAll_ReturnsGetBookDtoList()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto firstBook = GetAddBookDto();
            AddBookDto secondBook = GetAnotherAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);

            bookService.AddBook(firstBook);
            bookService.AddBook(secondBook);

            ServiceResponse<List<GetBookDto>> getAllBooksResponse = bookService.GetAllBooks();

            Assert.That(getAllBooksResponse.Success, Is.EqualTo(true));
            Assert.That(getAllBooksResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(getAllBooksResponse.Data, Is.InstanceOf<List<GetBookDto>>());
            Assert.That(getAllBooksResponse.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public void UpdateBook_ExistingBook_ReturnsGetBookDto()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = bookService.AddBook(newBook);
            GetBookDto addedBook = addBookResponse.Data.First();

            UpdateBookDto updatedBook = mapper.Map<UpdateBookDto>(addedBook);
            updatedBook.Price = 900;

            ServiceResponse<GetBookDto> updatedBookResponse = bookService.UpdateBook(updatedBook);
            GetBookDto savedBook = updatedBookResponse.Data;

            Assert.That(updatedBookResponse.Success, Is.EqualTo(true));
            Assert.That(updatedBookResponse, Is.InstanceOf<ServiceResponse<GetBookDto>>());
            Assert.That(savedBook, Is.InstanceOf<GetBookDto>());
            Assert.That(savedBook.Price, Is.EqualTo(updatedBook.Price));
        }

        [Test]
        public void UpdateBook_NonExistingBook_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UpdateBookDto updatedBook = GetUpdateBookDto();

            BookService bookService = new BookService(mapper, dbContext);

            ServiceResponse<GetBookDto> updatedBookResponse = bookService.UpdateBook(updatedBook);
            GetBookDto savedBook = updatedBookResponse.Data;

            Assert.That(updatedBookResponse.Success, Is.EqualTo(false));
            Assert.That(updatedBookResponse, Is.InstanceOf<ServiceResponse<GetBookDto>>());
            Assert.That(updatedBookResponse.Data, Is.EqualTo(null));
        }

        [Test]
        public void DeleteBook_ExistingBook_ReturnsGetBookDtoList()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = GetAddBookDto();

            BookService bookService = new BookService(mapper, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = bookService.AddBook(newBook);
            GetBookDto addedBook = addBookResponse.Data.First();

            ServiceResponse<List<GetBookDto>> deleteBookResponse = bookService.DeleteBook(addedBook.Id);

            Assert.That(deleteBookResponse.Success, Is.EqualTo(true));
            Assert.That(deleteBookResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(deleteBookResponse.Data, Is.InstanceOf<List<GetBookDto>>());
            Assert.That(deleteBookResponse.Data.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteBook_NonExistingBook_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            BookService bookService = new BookService(mapper, dbContext);

            ServiceResponse<List<GetBookDto>> deleteBookResponse = bookService.DeleteBook(1);

            Assert.That(deleteBookResponse.Success, Is.EqualTo(false));
            Assert.That(deleteBookResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(deleteBookResponse.Data, Is.EqualTo(null));
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
                Name = "Delivering Happiness",
                Author = "Tony Hsieh"
            };
        }

        private AddBookDto GetAnotherAddBookDto()
        {
            return new AddBookDto
            {
                Name = "Introduction to Algorithms",
                Author = "Thomas H. Cormen",
                CategoryId = 2
            };
        }
    }
}