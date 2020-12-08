using System.Collections.Generic;
using System.Threading.Tasks;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResponse<List<GetBookDto>>> GetAllBooks();
        Task<ServiceResponse<GetBookDto>> GetBookById(int id);
        Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto book);
        Task<ServiceResponse<GetBookDto>> UpdateBook(UpdateBookDto updateBook);
        Task<ServiceResponse<List<GetBookDto>>> DeleteBook(int id);
    }
}
