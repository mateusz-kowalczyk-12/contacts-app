using ContactsApp_Models.Responses;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Pages;

public partial class NewContactPage
{
    [Inject] public AppState AppState { get; set; } = null!;
}