using Books_Inventory_System.Dtos.Book;
using Books_Inventory_System.Models;
using AutoMapper;

namespace Books_Inventory_System
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, GetBookDto>();
            CreateMap<AddBookDto, Book>();
        }
    }
}
