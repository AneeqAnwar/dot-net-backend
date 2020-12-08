using AutoMapper;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    public class BookServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddBookDtoToBookMappingTest()
        {
            AutoMapperProfile myProfile = new AutoMapperProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            AddBookDto addBookDto = new AddBookDto();
            addBookDto.Name = "Delivering Happiness";
            //addBookDto.Author = "Tony Hseih";

            Book newBook = mapper.Map<Book>(addBookDto);

            Assert.That(newBook.Author, Is.EqualTo("Seth Godin"));
        }
    }
}