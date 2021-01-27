using System.Collections.Generic;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Services.BookService
{
    public interface IBookService
    {
        ServiceResponse<List<GetBookDto>> GetAllBooks();
        ServiceResponse<GetBookDto> GetBookById(int id);
        ServiceResponse<List<GetBookDto>> AddBook(AddBookDto book);
        ServiceResponse<GetBookDto> UpdateBook(UpdateBookDto updateBook);
        ServiceResponse<List<GetBookDto>> DeleteBook(int id);
    }
}
