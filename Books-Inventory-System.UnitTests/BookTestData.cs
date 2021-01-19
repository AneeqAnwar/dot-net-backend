using System.Collections.Generic;
using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;

namespace Books_Inventory_System.UnitTests
{
    public class BookTestData
    {
        public static AddBookDto AddBookDto()
        {
            return new AddBookDto
            {
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 899,
                CategoryId = 1
            };
        }

        public static UpdateBookDto AddBookDtoToUpdateBookDtoMapping()
        {
            return new UpdateBookDto
            {
                Id = 1,
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 899,
                CategoryId = 1
            };
        }

        public static GetBookDto AddBookDtoToGetBookDtoMapping()
        {
            return new GetBookDto
            {
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 900,
                CategoryId = 1
            };
        }

        public static Book Book()
        {
            return new Book
            {
                Id = 1,
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 899,
                CategoryId = 1
            };
        }

        public static GetBookDto BookMapping()
        {
            return new GetBookDto
            {
                Id = 1,
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 899,
                CategoryId = 1
            };
        }

        public static GetBookDto GetBookDto()
        {
            return new GetBookDto
            {
                Id = 1,
                Name = "Delivering Happiness",
                Description = "zappos.com",
                Author = "Tony Hsieh",
                Price = 899,
                CategoryId = 1
            };
        }

        public static UpdateBookDto UpdateBookDto()
        {
            return new UpdateBookDto
            {
                Name = "Delivering Happiness",
                Author = "Tony Hsieh"
            };
        }

        public static AddBookDto SecondAddBookDto()
        {
            return new AddBookDto
            {
                Name = "Introduction to Algorithms",
                Author = "Thomas H. Cormen",
                CategoryId = 2
            };
        }

        public static Book SecondAddBookDtoMapping()
        {
            return new Book
            {
                Name = "Introduction to Algorithms",
                Author = "Thomas H. Cormen",
                CategoryId = 2
            };
        }

        public static GetBookDto SecondAddBookDtoToGetBookDtoMapping()
        {
            return new GetBookDto
            {
                Name = "Introduction to Algorithms",
                Author = "Thomas H. Cormen",
                CategoryId = 2
            };
        }

        public static ServiceResponse<List<GetBookDto>> AddBookServiceResponse()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            List<GetBookDto> getBookDtos = new List<GetBookDto>
            {
                GetBookDto()
            };

            serviceResponse.Data = getBookDtos;
            serviceResponse.Success = true;
            serviceResponse.Message = "Book Added Successfully";

            return serviceResponse;
        }

        public static ServiceResponse<GetBookDto> GetSingleBookServiceResponse()
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>
            {
                Data = GetBookDto(),
                Success = true,
                Message = "Success"
            };

            return serviceResponse;
        }

        public static ServiceResponse<List<GetBookDto>> GetAllBooksServiceResponse()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            List<GetBookDto> getBookDtos = new List<GetBookDto>
            {
                GetBookDto()
            };

            serviceResponse.Data = getBookDtos;
            serviceResponse.Success = true;
            serviceResponse.Message = "Success";

            return serviceResponse;
        }

        public static ServiceResponse<GetBookDto> UpdateBookServiceResponse()
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>
            {
                Data = GetBookDto(),
                Success = true,
                Message = "Success"
            };

            return serviceResponse;
        }

        public static ServiceResponse<GetBookDto> UpdateBookServiceResponseNullData()
        {
            ServiceResponse<GetBookDto> serviceResponse = new ServiceResponse<GetBookDto>
            {
                Data = null,
                Success = false,
                Message = "Requested book not found"
            };

            return serviceResponse;
        }

        public static ServiceResponse<List<GetBookDto>> DeleteBookServiceResponse()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>();

            List<GetBookDto> getBookDtos = new List<GetBookDto>();

            serviceResponse.Data = getBookDtos;
            serviceResponse.Success = true;
            serviceResponse.Message = "Success";

            return serviceResponse;
        }

        public static ServiceResponse<List<GetBookDto>> DeleteBookServiceResponseNullData()
        {
            ServiceResponse<List<GetBookDto>> serviceResponse = new ServiceResponse<List<GetBookDto>>
            {
                Data = null,
                Success = true,
                Message = "Success"
            };

            return serviceResponse;
        }
    }
}
