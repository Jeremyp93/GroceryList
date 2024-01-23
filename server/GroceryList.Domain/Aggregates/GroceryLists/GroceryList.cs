using GroceryList.Domain.Events.GroceryLists;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.GroceryLists;

public class GroceryList : AggregateRoot
{
    public string Name { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? StoreId { get; private set; }

    private List<Ingredient> _ingredients = new();

    public IReadOnlyCollection<Ingredient> Ingredients
    {
        get
        {
            return _ingredients.AsReadOnly();
        }
        private set
        {
            _ingredients = value.ToList();
        }
    }

    private GroceryList()
    {
        /* private constructor only for EF */
    }

    public static GroceryList Create(Guid id, string name, Guid userId, Guid? storeId, List<Ingredient>? ingredients = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new BusinessValidationException("Grocery List is invalid", new() { "Name cannot be empty" });
        }

        var newGroceryList = new GroceryList()
        {
            Id = id,
            Name = name,
            UserId = userId,
            StoreId = storeId,
        };

        if (ingredients is not null)
        {
            newGroceryList.Ingredients = ingredients;
        }

        newGroceryList.AddDomainEvent(new GroceryListAddedEvent(newGroceryList.Id));
        return newGroceryList;
    }

    public void Update(string name, Guid userId, Guid? storeId, List<Ingredient>? ingredients = null) 
    { 
        Name = name;
        UserId = userId;
        StoreId = storeId;

        if (ingredients is not null)
        {
            Ingredients = ingredients;
        }
    }

    public void AssignIngredient(Ingredient newIngredient)
    {
        if (_ingredients.Any(ingredient => ingredient.Name.Equals(newIngredient.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            return;
        }

        _ingredients.Add(newIngredient);

        AddDomainEvent(new IngredientAssignedEvent(Id, newIngredient));
    }

    public void UpdateIngredient(Ingredient updatedIngredient)
    {
        _ingredients.RemoveAll(ingredient => ingredient.Name.Equals(updatedIngredient.Name, StringComparison.InvariantCultureIgnoreCase));
        _ingredients.Add(updatedIngredient);

        AddDomainEvent(new IngredientAssignedEvent(Id, updatedIngredient));
    }

    public void UpdateIngredients(List<Ingredient>? updatedIngredients)
    {
        _ingredients.Clear();
        if (updatedIngredients != null)
        {
            _ingredients.AddRange(updatedIngredients);
        }
        
        //AddDomainEvent(new IngredientAssignedEvent(Id, updatedIngredient));
    }
}
