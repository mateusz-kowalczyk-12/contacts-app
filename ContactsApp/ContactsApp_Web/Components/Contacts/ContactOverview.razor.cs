using Blazored.LocalStorage;
using ContactsApp_Models.Responses;
using ContactsApp_Web.GlobalHelpers;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Contacts;

public partial class ContactOverview
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private AppState AppState { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;

    
    private HttpClient _httpClient = new();
    
    [Parameter] public int Index { get; set; }
    [Parameter] public GetContactResponse Contact { get; set; } = null!;
    [Parameter] public GetContactResponse? SelectedContact { get; set; }


    protected override void OnInitialized()
    {
        _httpClient.BaseAddress = new Uri("http://host.docker.internal:8080");
    }


    private void SelectContact()
    {
        AppState.SelectedContact = Contact;
        NavigationManager.NavigateTo("/ContactDetails");
    }

    private async void DeleteContactAsync()
    {
        // Add the Bearer token to the request
        await AccessTokenHelper.LoadBearerTokenAsync(LocalStorage, _httpClient);
        
        await _httpClient.DeleteAsync("contacts/" + Contact.Id);
    }

    private void EditContact()
    {
        AppState.SelectedContact = Contact;
        NavigationManager.NavigateTo("/EditContact");
    }
}