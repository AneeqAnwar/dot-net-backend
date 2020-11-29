using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Services.BookService
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<List<Book>> AddBook(Book book);
    }
}
