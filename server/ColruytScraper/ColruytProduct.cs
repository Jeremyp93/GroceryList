namespace ColruytScraper;
public class ColruytProduct
{
    public Guid Id { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public double Price { get; set; }
    public ColruytCategory? Category { get; set; }
}

public class ColruytCategory
{
    public string Name { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
}
