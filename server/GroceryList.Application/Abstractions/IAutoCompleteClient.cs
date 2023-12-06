using GroceryList.Application.Queries.Geocode;

namespace GroceryList.Application.Abstractions;

public interface IAutoCompleteClient
{
    Task<IEnumerable<AutocompleteResponse>> AutoComplete(string searchText);
}
