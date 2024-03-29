﻿using GroceryList.Application.Abstractions;
using GroceryList.Application.Helpers;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.AddGroceryList;

public class AddGroceryListHandler : IRequestHandler<AddGroceryListCommand, Result<GroceryListResponseDto>>
{
    private readonly IGroceryListRepository _repository;
    private readonly IStoreRepository _storeRepository;

    public AddGroceryListHandler(IGroceryListRepository repository, IStoreRepository storeRepository)
    {
        _repository = repository;
        _storeRepository = storeRepository;
    }

    public async Task<Result<GroceryListResponseDto>> Handle(AddGroceryListCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var ingredients = command
              .Ingredients?
              .Select(x => Ingredient.Create(x.Name, x.Amount, Category.Create(x.Category), x.Selected))
              .ToList();

            var newGroceryList = Domain.Aggregates.GroceryLists.GroceryList.Create(Guid.NewGuid(), command.Name, command.StoreId, ingredients);

            var result = await _repository.AddAsync(newGroceryList);

            var newList = result.ToGroceryListDto();
            if (result.StoreId is not null)
            {
                newList.Store = await _storeRepository.GetByIdAsync((Guid)result.StoreId);
            }

            return Result<GroceryListResponseDto>.Success(newList);
        }
        catch (BusinessValidationException e)
        {
            return Result<GroceryListResponseDto>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}
