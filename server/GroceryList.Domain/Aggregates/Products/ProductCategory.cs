namespace GroceryList.Domain.Aggregates.Products;
public record ProductCategory
{
    public string Id { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    private ProductCategory() { }
}
