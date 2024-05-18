using Blazored.LocalStorage;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Pages;

public partial class LogoutPage
{
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    
    private bool _isSuccess = false;
    
    
    private async Task LogoutAsync()
    {
        // Reset the success state
        _isSuccess = false;
        
        // Delete the Bearer token
        await LocalStorage.RemoveItemAsync("accessTokenResponse");
        
        // Set the success state
        _isSuccess = true;
    }
}