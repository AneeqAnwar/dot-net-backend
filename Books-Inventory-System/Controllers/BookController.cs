using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("GetAll")]
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
        public async Task<IActionResult> AddBook(Book book)
        {
            return Ok(await bookService.AddBook(book));
        }
    }
}
