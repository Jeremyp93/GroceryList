using AutoMapper;
using GroceryList.Domain.Aggregates.Items;
using GroceryList.WebApi.Models.Items;

namespace GroceryList.WebApi.Mappers;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemResponse>();
    }
}
