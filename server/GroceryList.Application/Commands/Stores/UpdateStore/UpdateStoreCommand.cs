using GroceryList.Domain.Aggregates.Stores;
using MediatR;

namespace GroceryList.Application.Commands.Stores.UpdateStore;
public record UpdateStoreCommand() : IRequest<Result<Store>>
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Street { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? ZipCode { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public List<SectionDto> Sections { get; set; } = new List<SectionDto>();
}
