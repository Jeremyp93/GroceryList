using GroceryList.Application.Queries.GroceryLists;
using MediatR;

namespace GroceryList.Application.Queries.Geocode.Autocomplete;

public record AutoCompleteQuery(string SearchText) : IRequest<Result<IEnumerable<AutocompleteResponse>>>;
