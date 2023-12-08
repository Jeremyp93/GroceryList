using GroceryList.Domain.Aggregates.Stores;
using MediatR;

namespace GroceryList.Application.Commands.Stores.UpdateSections;
public record UpdateSectionsCommand : IRequest<Result<List<Section>>>
{
    public Guid StoreId { get; set; } = Guid.Empty;
    public List<SectionDto>? Sections { get; set; }
}
