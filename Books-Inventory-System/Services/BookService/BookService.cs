using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Services.BookService
{
    public class BookService : IBookService
    {
        public BookService()
        {
        }

        private static List<Book> books = new List<Book>()
        {
            new Book(),
            new Book{Id = 1, Name = "Delivering Happiness", Author = "Tony Hsieh", Description = "Zappos.com"}
        };

        public async Task<List<Book>> AddBook(Book book)
        {
            books.Add(book);
            return books;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return books;
        }

        public async Task<Book> GetBookById(int id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }
    }
}
