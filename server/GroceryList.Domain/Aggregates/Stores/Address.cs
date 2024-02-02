using GroceryList.Domain.Exceptions;

namespace GroceryList.Domain.Aggregates.Stores;

public record Address
{
    private const int MaxStreetLength = 50;
    private const int MaxCityLength = 15;
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;

    private Address() { }

    public static Address? Create(string? street, string? city, string? country, string? zipCode)
    {
        if (string.IsNullOrEmpty(street) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(country) && string.IsNullOrEmpty(zipCode))
        {
            return null;
        }

        var errors = new List<string>();

        if (street?.Length >= MaxStreetLength)
        {
            errors.Add($"Street length is higher than the limit ({street?.Length} - MAX: {MaxStreetLength})");
        }

        if (city?.Length >= MaxCityLength)
        {
            errors.Add($"City length is higher than the limit ({city.Length} - MAX: {MaxCityLength})");
        }

        if (zipCode?.Length < 2 || zipCode?.Length > 7)
        {
            errors.Add($"Zip code should be between 2 and 7 characters ({zipCode?.Length})");
        }

        if (errors.Count != 0)
        {
            throw new BusinessValidationException("Address is invalid", errors);
        }

        var address = new Address()
        {
            Street = street!,
            City = city!,
            Country = country!,
            ZipCode = zipCode!
        };
        return address;
    }
}


