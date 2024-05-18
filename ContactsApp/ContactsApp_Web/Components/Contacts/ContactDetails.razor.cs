using ContactsApp_Models.Responses;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Contacts;

public partial class ContactDetails
{
    [Inject] private AppState AppState { get; set; } = null!;
}