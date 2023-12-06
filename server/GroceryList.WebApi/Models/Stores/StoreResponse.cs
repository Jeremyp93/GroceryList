using Microsoft.AspNetCore.SignalR;

namespace GroceryList.WebApi.Models.Stores;

public class StoreResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid UserId { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public List<SectionResponse>? Sections { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}
