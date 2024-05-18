using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.GlobalHelpers;

public class AccessTokenHelper
{
    /// Returns true if the token has been loaded. Otherwise returns false
    public static async Task<bool> LoadBearerTokenAsync(ILocalStorageService localStorage, HttpClient httpClient)
    {
        var accessTokenModel = await localStorage.GetItemAsync<AccessTokenResponse>("accessTokenResponse");
        if (accessTokenModel == null)
            return false;
        
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessTokenModel.AccessToken);

        return true;
    }
}