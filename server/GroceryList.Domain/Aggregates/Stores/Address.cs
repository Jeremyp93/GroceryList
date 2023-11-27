using GroceryList.Domain.Exceptions;

namespace GroceryList.Domain.Aggregates.Stores;

public record Address
{
    private const int MaxStreetLength = 50;
    private const int MaxCityLength = 15;
    private const int MaxStateLength = 15;
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    private Address() { }

    public static Address Create(string street, string city, string state, string country, string zipCode)
    {
        var errors = new List<string>();

        if (street?.Length >= MaxStreetLength)
        {
            errors.Add($"Street length is higher than the limit ({street?.Length} - MAX: {MaxStreetLength})");
        }

        if (city?.Length >= MaxCityLength)
        {
            errors.Add($"City length is higher than the limit ({city.Length} - MAX: {MaxCityLength})");
        }

        if (state?.Length >= MaxStateLength)
        {
            errors.Add($"State length is higher than the limit ({state?.Length} - MAX: {MaxStateLength})");
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
            Street = street,
            City = city,
            State = state,
            Country = country,
            ZipCode = zipCode
        };
        return address;
    }
}


