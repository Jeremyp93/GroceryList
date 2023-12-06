using AutoMapper;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.WebApi.Models.Stores;

namespace GroceryList.WebApi.Mappers;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Section, SectionResponse>();

        CreateMap<Store, StoreResponse>()
        .ForMember(d => d.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
        .ForMember(d => d.Street, opt => opt.MapFrom(src => src.Address.Street))
        .ForMember(d => d.City, opt => opt.MapFrom(src => src.Address.City))
        .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Address.Country));
    }
}
