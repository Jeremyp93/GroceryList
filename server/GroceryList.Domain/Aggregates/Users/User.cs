using GroceryList.Domain.Events.Users;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Aggregates.Users;

public class User : AggregateRoot
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Password { get; private set; }
    public bool EmailConfirmed { get; private set; }

    private List<OAuthProvider> _oauthProviders = new List<OAuthProvider>();

    public IReadOnlyCollection<OAuthProvider> OAuthProviders
    {
        get
        {
            return _oauthProviders.AsReadOnly();
        }
        private set
        {
            _oauthProviders = value.ToList();
        }
    }

    private User()
    {
        /* private constructor only for EF */
    }

    public static User Create(Guid id, string? firstName, string? lastName, string email, string? password, List<OAuthProvider>? oauthProviders = null)
    {
        var newUser = new User()
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        if (oauthProviders is not null)
        {
            newUser._oauthProviders = oauthProviders;
        }

        newUser.AddDomainEvent(new UserAddedEvent(newUser.Id));
        return newUser;
    }

    public void AddOAuthProvider(OAuthProvider oauthProvider)
    {
        if (_oauthProviders.Any(provider => provider.OAuthProviderId.Equals(oauthProvider.OAuthProviderId, StringComparison.InvariantCultureIgnoreCase) && provider.OAuthProviderName.Equals(oauthProvider.OAuthProviderName, StringComparison.InvariantCultureIgnoreCase)))
        {
            return;
        }

        _oauthProviders.Add(oauthProvider);
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }
}
