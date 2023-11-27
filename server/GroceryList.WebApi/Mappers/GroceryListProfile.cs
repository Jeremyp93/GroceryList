using AutoMapper;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.WebApi.Models.GroceryLists;

namespace GroceryList.WebApi.Mappers;

public class GroceryListProfile : Profile
{
    public GroceryListProfile()
    {
        CreateMap<Ingredient, IngredientResponse>()
            .ForMember(i => i.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<GroceryListResponseDto, GroceryListResponse>();
    }
}
