using System.Collections.Generic;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using Books_Inventory_System.Services.BookService;
using Microsoft.AspNetCore.Mvc;

namespace Books_Inventory_System.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookController: ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleBook(int id)
        {
            return Ok(bookService.GetBookById(id));
        }

        [HttpPost]
        public IActionResult AddBook(AddBookDto book)
        {
            return Ok(bookService.AddBook(book));
        }

        [HttpPut]
        public IActionResult UpdateBook(UpdateBookDto updatedBook)
        {
            ServiceResponse<GetBookDto> response = bookService.UpdateBook(updatedBook);

            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            ServiceResponse<List<GetBookDto>> response = bookService.DeleteBook(id);

            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Status")]
        public IActionResult CheckStatus()
        {
            return Ok("The book inventory is up and running!!!");
        }
    }
}
