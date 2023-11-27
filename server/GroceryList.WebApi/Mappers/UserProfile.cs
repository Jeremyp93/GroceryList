using AutoMapper;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.WebApi.Models.Users;

namespace GroceryList.WebApi.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
    }
}
