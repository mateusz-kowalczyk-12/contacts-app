using ContactsApp_Models.Responses;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Contacts;

public partial class ContactOverview
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private AppState AppState { get; set; } = null!;
    [Parameter] public int Index { get; set; }
    [Parameter] public GetContactResponse Contact { get; set; } = null!;
    [Parameter] public GetContactResponse? SelectedContact { get; set; }


    private void SelectContact()
    {
        AppState.SelectedContact = Contact;
        NavigationManager.NavigateTo("/ContactDetails");
    }
}