using GroceryList.Application.Queries.GroceryLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Application.Helpers;
internal static class GroceryListMapper
{
    public static GroceryListResponseDto ToGroceryListDto(this Domain.Aggregates.GroceryLists.GroceryList list)
    {
        return new GroceryListResponseDto
        {
            Id = list.Id,
            Name = list.Name,
            UserId = list.UserId,
            Ingredients = list.Ingredients,
            CreatedAt = list.CreatedAt
        };
    }
}
