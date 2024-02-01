using AutoMapper;
using GroceryList.Application.Models;
using GroceryList.WebApi.Models.Users;

namespace GroceryList.WebApi.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserResponse>();
    }
}
