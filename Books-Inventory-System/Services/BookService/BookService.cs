using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_Inventory_System.Services.BookService
{
    public class BookService : IBookService
    {
        public BookService(IMapper mapper, DataContext dataContext)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        private static List<Book> books = new List<Book>()
        {
            new Book(),
            new Book{Id = 1, Name = "Delivering Happiness", Author = "Tony Hsieh", Description = "Zappos.com"}
        };

        private readonly IMapper mapper;
        private readonly DataContext dataContext;

        public async Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto book)
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();
            Book newBook = mapper.Map<Book>(book);
            await dataContext.Books.AddAsync(newBook);
            await dataContext.SaveChangesAsync();

            serviceResponse.Data = dataContext.Books.Select(b => mapper.Map<GetBookDto>(b)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> GetAllBooks()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            List<Book> dbBooks = await dataContext.Books.ToListAsync();
            serviceResponse.Data = dbBooks.Select(b => mapper.Map<GetBookDto>(b)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBookDto>> GetBookById(int id)
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>();
            Book dbBook = await dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            serviceResponse.Data = mapper.Map<GetBookDto>(dbBook);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBookDto>> UpdateBook(UpdateBookDto updateBook)
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>();

            try
            {
                Book foundBook = await dataContext.Books.FirstOrDefaultAsync(b => b.Id == updateBook.Id);
                foundBook.Name = updateBook.Name;
                foundBook.Description = updateBook.Description;
                foundBook.Author = updateBook.Author;
                foundBook.Price = updateBook.Price;
                foundBook.CategoryId = updateBook.CategoryId;

                dataContext.Books.Update(foundBook);
                await dataContext.SaveChangesAsync();

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
                Book foundBook = await dataContext.Books.FirstAsync(b => b.Id == id);
                dataContext.Books.Remove(foundBook);

                await dataContext.SaveChangesAsync();

                serviceResponse.Data = dataContext.Books.Select(b => mapper.Map<GetBookDto>(b)).ToList();
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
