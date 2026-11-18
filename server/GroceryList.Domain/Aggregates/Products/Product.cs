using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Products;
public class Product : AggregateRoot
{
    public string ProductId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public string Thumbnail { get; private set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public double Price { get; private set; }
    public ProductCategory? Category { get; private set; }
}
