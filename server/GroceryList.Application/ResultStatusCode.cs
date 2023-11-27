namespace GroceryList.Application
{
    public enum ResultStatusCode
    {
        None,
        ValidationError,
        NotFound,
        Error
        // to be enhanced, add the status code you need (based on the logic you implement (expected errors to handle) in your handler)
    }
}
