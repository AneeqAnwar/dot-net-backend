using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get()
        {
            return Ok(await bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleBook(int id)
        {
            return Ok(await bookService.GetBookById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookDto book)
        {
            return Ok(await bookService.AddBook(book));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(UpdateBookDto updatedBook)
        {
            ServiceResponse<GetBookDto> response = await bookService.UpdateBook(updatedBook);

            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            ServiceResponse<List<GetBookDto>> response = await bookService.DeleteBook(id);

            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Status")]
        public IActionResult CheckStatus()
        {
            return Ok("The book inventory is up and running!");
        }
    }
}
