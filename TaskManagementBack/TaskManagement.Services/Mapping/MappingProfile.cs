using AutoMapper;
using TaskManagement.DataAccess.Entities;
using TaskManagement.Services.DTOs;

namespace TaskManagement.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductsEntity, ProductDTO>();
            CreateMap<ProductDTO, ProductsEntity>();
        }
    }
}