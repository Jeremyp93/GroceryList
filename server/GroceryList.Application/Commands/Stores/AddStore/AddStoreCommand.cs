using GroceryList.Domain.Aggregates.Stores;
using MediatR;

namespace GroceryList.Application.Commands.Stores.AddStores;

public record AddStoreCommand() : IRequest<Result<Store>>
{
    public string Name { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public List<SectionDto>? Sections { get; set; }
}
