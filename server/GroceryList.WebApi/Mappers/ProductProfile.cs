using AutoMapper;
using GroceryList.Domain.Aggregates.Products;
using GroceryList.WebApi.Models.Products;

namespace GroceryList.WebApi.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCategory, CategoryResponse>();
        CreateMap<Product, ProductResponse>()
            .ForMember(x => x.CreatedOn, opt => opt.MapFrom(src => src.CreatedAt));
    }
}