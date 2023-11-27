using GroceryList.Domain.Aggregates.GroceryLists;

namespace GroceryList.Domain.Events.GroceryLists;

public record IngredientAssignedEvent : IDomainEvent
{
    public Guid GrocerListId { get; private set; }
    public Ingredient Ingredient { get; private set; }

    public IngredientAssignedEvent(Guid grocerListId, Ingredient ingredient)
    {
        GrocerListId = grocerListId;
        Ingredient = ingredient;
    }
}
