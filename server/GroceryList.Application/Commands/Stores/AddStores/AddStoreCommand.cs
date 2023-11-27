using GroceryList.Domain.Aggregates.Stores;
using MediatR;

namespace GroceryList.Application.Commands.Stores.AddStores;

public record AddStoreCommand() : IRequest<Result<Store>>
{
    public string Name { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<SectionDto> Sections { get; set; }
}
