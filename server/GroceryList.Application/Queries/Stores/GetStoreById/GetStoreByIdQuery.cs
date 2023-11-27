using GroceryList.Domain.Aggregates.Stores;
using MediatR;

namespace GroceryList.Application.Queries.Stores.GetStoreById;

public record GetStoreByIdQuery(Guid id) : IRequest<Result<Store>>;
