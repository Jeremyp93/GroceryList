using GroceryList.Application.Abstractions;
using GroceryList.Application.Queries.Geocode;
using GroceryList.Application.Queries.Geocode.Autocomplete;
using MediatR;

namespace GroceryList.Application.Queries.GroceryLists.GetGroceryLists;

public class AutoCompleteHandler : IRequestHandler<AutoCompleteQuery, Result<IEnumerable<AutocompleteResponse>>>
{
    private readonly IAutoCompleteClient _autocompleteClient;

    public AutoCompleteHandler(IAutoCompleteClient autocompleteClient)
    {
        _autocompleteClient = autocompleteClient;
    }

    public async Task<Result<IEnumerable<AutocompleteResponse>>> Handle(AutoCompleteQuery request, CancellationToken cancellationToken)
    {
        var results = await _autocompleteClient.AutoComplete(request.SearchText);

        return Result<IEnumerable<AutocompleteResponse>>.Success(results);
    }

}
