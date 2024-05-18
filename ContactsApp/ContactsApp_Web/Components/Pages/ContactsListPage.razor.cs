using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Pages;

public partial class ContactsListPage
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private AppState AppState { get; set; } = null!;


    protected override async Task OnInitializedAsync()
    {
        await AppState.FetchContactsAsync();
        await AppState.FetchCategoriesAsync();
    }
}