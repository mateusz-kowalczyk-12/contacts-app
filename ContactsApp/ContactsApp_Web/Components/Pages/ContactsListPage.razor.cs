using ContactsApp_Models.Responses;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Pages;

public partial class ContactsListPage
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private AppState AppState { get; set; } = null!;
    private readonly HttpClient _httpClient = new();


    protected override async Task OnInitializedAsync()
    {
        try
        {
            AppState.Contacts = await _httpClient.GetFromJsonAsync<List<GetContactResponse>>(
                "http://host.docker.internal:8080/contacts");
            AppState.MainCategories = await _httpClient.GetFromJsonAsync<List<GetCategoryResponse>>(
                "http://host.docker.internal:8080/contacts/categories?mainCategories=true");
            AppState.BusinessSubcategories = await _httpClient.GetFromJsonAsync<List<GetCategoryResponse>>(
                "http://host.docker.internal:8080/contacts/categories?mainCategories=false");
        }
        catch (HttpRequestException)
        {
            NavigationManager.NavigateTo("/Error");
        }
    }
}