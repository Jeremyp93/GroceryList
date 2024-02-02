using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Events.Stores;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Stores;

public class Store : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public Address? Address { get; private set; }

    private List<Section> _sections = new();

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

    public static Store Create(Guid id, string name, List<Section>? sections, Address? address = null)
    {
        var newStore = new Store()
        {
            Id = id,
            Name = name,
            Address = address
        };

        if (sections is not null)
        {
            newStore.Sections = sections;
        }

        newStore.AddDomainEvent(new StoreAddedEvent(newStore.Id));
        return newStore;
    }

    public void Update(string name, List<Section>? sections, Address? address = null)
    {
        Name = name;
        if (address is not null)
        {
            Address = address;
        }

        if (sections is not null)
        {
            Sections = sections;
        }
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

    public void UpdateAddress(string street, string city, string country, string zipCode)
    {
        if (street != null || city != null || country != null || zipCode != null)
        {
            Address = Address.Create(street, city, country, zipCode);
        }
        else
        {
            Address = null;
        }
    }

    public void UpdateSections(List<Section>? updatedSections)
    {
        _sections.Clear();
        if (updatedSections != null)
        {
            _sections.AddRange(updatedSections);
        }

        //AddDomainEvent(new IngredientAssignedEvent(Id, updatedIngredient));
    }
}
