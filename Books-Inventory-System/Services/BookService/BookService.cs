using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Services.BookService
{
    public class BookService : IBookService
    {
        public BookService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        private static List<Book> books = new List<Book>()
        {
            new Book(),
            new Book{Id = 1, Name = "Delivering Happiness", Author = "Tony Hsieh", Description = "Zappos.com"}
        };
        private readonly IMapper mapper;

        public async Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto book)
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();
            Book newBook = mapper.Map<Book>(book);
            newBook.Id = books.Max(b => b.Id) + 1;
            books.Add(newBook);
            serviceResponse.Data = books.Select(b => mapper.Map<GetBookDto>(b)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> GetAllBooks()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();
            serviceResponse.Data = books.Select(b => mapper.Map<GetBookDto>(b)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBookDto>> GetBookById(int id)
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>();
            serviceResponse.Data = mapper.Map<GetBookDto>(books.FirstOrDefault(b => b.Id == id));

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBookDto>> UpdateBook(UpdateBookDto updateBook)
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>();

            try
            {
                Book foundBook = books.FirstOrDefault(b => b.Id == updateBook.Id);
                foundBook.Name = updateBook.Name;
                foundBook.Description = updateBook.Description;
                foundBook.Author = updateBook.Author;
                foundBook.Price = updateBook.Price;
                foundBook.CategoryId = updateBook.CategoryId;

                serviceResponse.Data = mapper.Map<GetBookDto>(foundBook);
                serviceResponse.Message = "Book updated successfully";
            }
            catch (Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Requested book not found";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> DeleteBook(int id)
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            try
            {
                Book foundBook = books.First(b => b.Id == id);
                books.Remove(foundBook);

                serviceResponse.Data = books.Select(b => mapper.Map<GetBookDto>(b)).ToList();
                serviceResponse.Message = "Book deleted successfully";
            }
            catch (Exception)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Requested book not found";
            }

            return serviceResponse;
        }
    }
}
