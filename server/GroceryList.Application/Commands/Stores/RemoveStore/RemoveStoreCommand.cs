using MediatR;

namespace GroceryList.Application.Commands.Stores.RemoveStore;

public record RemoveStoreCommand(Guid Id) : IRequest<Result>;
