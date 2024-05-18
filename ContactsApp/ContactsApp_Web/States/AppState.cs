using ContactsApp_Models.Responses;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.States;

public class AppState
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private readonly HttpClient _httpClient = new();
    
    public List<GetContactResponse>? Contacts { get; set; } = [];
    public List<GetCategoryResponse>? MainCategories { get; set; } = [];
    public List<GetCategoryResponse>? BusinessSubcategories { get; set; } = [];
    
    public GetContactResponse? SelectedContact { get; set; }
    

    public async Task FetchContactsAsync()
    {
        try
        {
            Contacts = await _httpClient.GetFromJsonAsync<List<GetContactResponse>>(
                "http://host.docker.internal:8080/contacts");
        }
        catch (HttpRequestException)
        {
            NavigationManager.NavigateTo("/Error");
        }
    }
    
    public async Task FetchCategoriesAsync()
    {
        try
        {
            MainCategories = await _httpClient.GetFromJsonAsync<List<GetCategoryResponse>>(
                "http://host.docker.internal:8080/contacts/categories?mainCategories=true");
            BusinessSubcategories = await _httpClient.GetFromJsonAsync<List<GetCategoryResponse>>(
                "http://host.docker.internal:8080/contacts/categories?mainCategories=false");
        }
        catch (HttpRequestException)
        {
            NavigationManager.NavigateTo("/Error");
        }
    }
}