namespace GroceryList.Application.Abstractions;
public interface IClaimReader
{
    Guid GetUserIdFromClaim();

    string GetUsernameFromClaim();
}
