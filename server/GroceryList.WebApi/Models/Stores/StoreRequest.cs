using GroceryList.Application.Commands.Stores;
using System.ComponentModel.DataAnnotations;

namespace GroceryList.WebApi.Models.Stores;

public class StoreRequest
{
    public Guid? Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public List<SectionDto> Sections { get; set; }
}
