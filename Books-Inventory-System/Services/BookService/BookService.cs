using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Books_Inventory_System.Data;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.Services.BookService
{
    public class BookService : IBookService
    {
        public BookService(IMapper mapper, IDataContext dataContext)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
        }

        private readonly IMapper mapper;
        private readonly IDataContext dataContext;

        public ServiceResponse<List<GetBookDto>> AddBook(AddBookDto book)
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();
            Book newBook = mapper.Map<Book>(book);
            dataContext.Books.Add(newBook);
            dataContext.SaveChanges();

            serviceResponse.Data = dataContext.Books.Select(b => mapper.Map<GetBookDto>(b)).ToList();

            return serviceResponse;
        }

        public ServiceResponse<List<GetBookDto>> GetAllBooks()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            List<Book> dbBooks = dataContext.Books.ToList();
            serviceResponse.Data = dbBooks.Select(b => mapper.Map<GetBookDto>(b)).ToList();

            return serviceResponse;
        }

        public ServiceResponse<GetBookDto> GetBookById(int id)
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>();
            Book dbBook = dataContext.Books.FirstOrDefault(b => b.Id == id);
            serviceResponse.Data = mapper.Map<GetBookDto>(dbBook);

            return serviceResponse;
        }

        public ServiceResponse<GetBookDto> UpdateBook(UpdateBookDto updateBook)
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>();

            try
            {
                Book foundBook = dataContext.Books.FirstOrDefault(b => b.Id == updateBook.Id);
                foundBook.Name = updateBook.Name;
                foundBook.Description = updateBook.Description;
                foundBook.Author = updateBook.Author;
                foundBook.Price = updateBook.Price;
                foundBook.CategoryId = updateBook.CategoryId;

                dataContext.Books.Update(foundBook);
                dataContext.SaveChanges();

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

        public ServiceResponse<List<GetBookDto>> DeleteBook(int id)
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            try
            {
                Book foundBook = dataContext.Books.First(b => b.Id == id);
                dataContext.Books.Remove(foundBook);

                dataContext.SaveChanges();

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
