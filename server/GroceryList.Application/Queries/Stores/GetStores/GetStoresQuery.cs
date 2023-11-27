using GroceryList.Domain.Aggregates.Stores;
using MediatR;

namespace GroceryList.Application.Queries.Stores.GetStores;

public record GetStoresQuery() : IRequest<Result<IEnumerable<Store>>>;

