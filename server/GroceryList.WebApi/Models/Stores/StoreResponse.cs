namespace GroceryList.WebApi.Models.Stores;

public class StoreResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public List<SectionResponse>? Sections { get; set; }
}
