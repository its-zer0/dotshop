using AutoMapper;
using DotShop.API.Models.Domain;
using DotShop.API.Models.DTO;

namespace DotShop.API.Mappings;

public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {
        // This maps the properties of the Product domain model to the ProductResponseDTO and vice versa.
        CreateMap<Product, ProductResponseDTO>().ReverseMap();
        // It also maps the AddProductRequestDTO to the Product domain model and vice versa.
        CreateMap<AddProductRequestDTO, Product>().ReverseMap();

        CreateMap<UpdateProductRequestDTO, Product>().ReverseMap();

    }
}