﻿namespace GroceryList.Application.Commands.LoginGoogle;
public class GoogleOptions
{
    public string AuthorizationEndpoint { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string CallbackUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}