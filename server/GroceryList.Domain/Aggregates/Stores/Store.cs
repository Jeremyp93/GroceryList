using GroceryList.Domain.Events.Stores;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Stores;

public class Store : AggregateRoot
{
    public string Name { get; private set; }
    public Address? Address { get; private set; }

    private List<Section> _sections = new List<Section>();

    public IReadOnlyCollection<Section> Sections
    {
        get
        {
            return _sections.AsReadOnly();
        }
        private set
        {
            _sections = value.ToList();
        }
    }

    private Store()
    {
        /* private constructor only for EF */
    }

    public static Store Create(string name, List<Section> sections, Address? address = null)
    {
        var newStore = new Store()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Sections = sections,
            Address = address
        };

        newStore.AddDomainEvent(new StoreAddedEvent(newStore.Id));
        return newStore;
    }

    public void AssignSection(Section newSection)
    {
        if (_sections.Any(section => section.Name.Equals(newSection.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            return;
        }

        _sections.Add(newSection);

        //AddDomainEvent(new IngredientAssignedEvent(Id, newIngredient));
    }

    public void UpdateSection(Section updatedSection)
    {
        _sections.RemoveAll(section => section.Name.Equals(updatedSection.Name, StringComparison.InvariantCultureIgnoreCase));
        _sections.Add(updatedSection);

        //AddDomainEvent(new IngredientAssignedEvent(Id, updatedSection)); 
    }

    public void UpdateAddress(string street, string city, string state, string country, string zipCode)
    {
        if (street != null || city != null || state != null || country != null || zipCode != null)
        {
            Address = Address.Create(street, city, state, country, zipCode);
        }
        else
        {
            Address = null;
        }
    }
}
