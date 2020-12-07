using Books_Inventory_System.Models;
using NUnit.Framework;

namespace Books_Inventory_System.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BookAuthorNameTest()
        {
            Book book = new Book();
            Assert.That(book.Author, Is.EqualTo("Seth Godin"));
        }
    }
}