using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using Books_Inventory_System.Services.BookService;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IMapper> mapperMock;
        DataContext dbContext;

        [SetUp]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "BooksDatabase")
            .Options;
            dbContext = new DataContext(options);
        }

        [Test]
        public async Task AddBook_NewBook_ReturnsGetBookDtoList()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            mapperMock.Setup(m => m.Map<Book>(It.IsAny<AddBookDto>())).Returns(BookTestData.Book());
            mapperMock.Setup(m => m.Map<GetBookDto>(It.IsAny<Book>())).Returns(BookTestData.GetBookDto());

            AddBookDto newBook = BookTestData.AddBookDto();

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = await bookService.AddBook(newBook);
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
        public async Task GetBookById_BookId_ReturnsGetBookDto()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            mapperMock.Setup(m => m.Map<Book>(It.IsAny<AddBookDto>())).Returns(BookTestData.Book());
            mapperMock.Setup(m => m.Map<GetBookDto>(It.IsAny<Book>())).Returns(BookTestData.GetBookDto());

            AddBookDto newBook = BookTestData.AddBookDto();

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = await bookService.AddBook(newBook);
            GetBookDto savedBook = addBookResponse.Data.First();

            ServiceResponse<GetBookDto> getBookResponse = await bookService.GetBookById(savedBook.Id);
            GetBookDto receivedBook = getBookResponse.Data;

            Assert.That(getBookResponse.Success, Is.EqualTo(true));
            Assert.That(getBookResponse, Is.InstanceOf<ServiceResponse<GetBookDto>>());
            Assert.That(getBookResponse.Data, Is.InstanceOf<GetBookDto>());
            Assert.That(receivedBook, Is.InstanceOf<GetBookDto>());
            Assert.That(receivedBook.Name, Is.EqualTo(savedBook.Name));
            Assert.That(receivedBook.Author, Is.EqualTo(savedBook.Author));
        }

        [Test]
        public async Task GetAllBooks_GetAll_ReturnsGetBookDtoList()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto firstBook = BookTestData.AddBookDto();
            AddBookDto secondBook = BookTestData.SecondAddBookDto();

            mapperMock.Setup(m => m.Map<Book>(firstBook)).Returns(BookTestData.Book());
            mapperMock.Setup(m => m.Map<Book>(secondBook)).Returns(BookTestData.SecondAddBookDtoMapping());
            mapperMock.Setup(m => m.Map<GetBookDto>(BookTestData.Book())).Returns(BookTestData.BookMapping());
            mapperMock.Setup(m => m.Map<GetBookDto>(BookTestData.SecondAddBookDtoMapping())).Returns(BookTestData.SecondAddBookDtoToGetBookDtoMapping());

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            await bookService.AddBook(firstBook);
            await bookService.AddBook(secondBook);

            ServiceResponse<List<GetBookDto>> getAllBooksResponse = await bookService.GetAllBooks();

            Assert.That(getAllBooksResponse.Success, Is.EqualTo(true));
            Assert.That(getAllBooksResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(getAllBooksResponse.Data, Is.InstanceOf<List<GetBookDto>>());
            Assert.That(getAllBooksResponse.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateBook_ExistingBook_ReturnsGetBookDto()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = BookTestData.AddBookDto();

            mapperMock.Setup(m => m.Map<Book>(It.IsAny<AddBookDto>())).Returns(BookTestData.Book());
            mapperMock.Setup(m => m.Map<UpdateBookDto>(It.IsAny<GetBookDto>())).Returns(BookTestData.AddBookDtoToUpdateBookDtoMapping());
            mapperMock.Setup(m => m.Map<GetBookDto>(It.IsAny<Book>())).Returns(BookTestData.AddBookDtoToGetBookDtoMapping());

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = await bookService.AddBook(newBook);
            GetBookDto addedBook = addBookResponse.Data.First();

            UpdateBookDto updatedBook = mapperMock.Object.Map<UpdateBookDto>(addedBook);
            updatedBook.Price = 900;

            ServiceResponse<GetBookDto> updatedBookResponse = await bookService.UpdateBook(updatedBook);
            GetBookDto savedBook = updatedBookResponse.Data;

            Assert.That(updatedBookResponse.Success, Is.EqualTo(true));
            Assert.That(updatedBookResponse, Is.InstanceOf<ServiceResponse<GetBookDto>>());
            Assert.That(savedBook, Is.InstanceOf<GetBookDto>());
            Assert.That(savedBook.Price, Is.EqualTo(updatedBook.Price));
        }

        [Test]
        public async Task UpdateBook_NonExistingBook_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            UpdateBookDto updatedBook = BookTestData.UpdateBookDto();

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            ServiceResponse<GetBookDto> updatedBookResponse = await bookService.UpdateBook(updatedBook);
            GetBookDto savedBook = updatedBookResponse.Data;

            Assert.That(updatedBookResponse.Success, Is.EqualTo(false));
            Assert.That(updatedBookResponse, Is.InstanceOf<ServiceResponse<GetBookDto>>());
            Assert.That(updatedBookResponse.Data, Is.EqualTo(null));
        }

        [Test]
        public async Task DeleteBook_ExistingBook_ReturnsGetBookDtoList()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddBookDto newBook = BookTestData.AddBookDto();

            mapperMock.Setup(m => m.Map<Book>(It.IsAny<AddBookDto>())).Returns(BookTestData.Book());
            mapperMock.Setup(m => m.Map<GetBookDto>(It.IsAny<Book>())).Returns(BookTestData.BookMapping());

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            ServiceResponse<List<GetBookDto>> addBookResponse = await bookService.AddBook(newBook);
            GetBookDto addedBook = addBookResponse.Data.First();

            ServiceResponse<List<GetBookDto>> deleteBookResponse = await bookService.DeleteBook(addedBook.Id);

            Assert.That(deleteBookResponse.Success, Is.EqualTo(true));
            Assert.That(deleteBookResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(deleteBookResponse.Data, Is.InstanceOf<List<GetBookDto>>());
            Assert.That(deleteBookResponse.Data.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteBook_NonExistingBook_ReturnsError()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            BookService bookService = new BookService(mapperMock.Object, dbContext);

            ServiceResponse<List<GetBookDto>> deleteBookResponse = await bookService.DeleteBook(1);

            Assert.That(deleteBookResponse.Success, Is.EqualTo(false));
            Assert.That(deleteBookResponse, Is.InstanceOf<ServiceResponse<List<GetBookDto>>>());
            Assert.That(deleteBookResponse.Data, Is.EqualTo(null));
        }
    }
}