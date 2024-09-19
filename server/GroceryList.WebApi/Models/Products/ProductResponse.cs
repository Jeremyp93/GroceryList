namespace GroceryList.WebApi.Models.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public double Price { get; set; }
    public CategoryResponse? Category { get; set; }
    public DateTime CreatedOn { get; set; }
}
