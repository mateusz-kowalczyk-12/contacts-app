using Microsoft.AspNetCore.Components;
using ContactsApp_Models.Responses;

namespace ContactsApp_Web.Components.Contacts;

public partial class ContactsList
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private HttpClient _httpClient = new();
    private List<GetContactResponse>? _contacts = [];
    private List<GetCategoryResponse>? _mainCategories = [];
    private List<GetCategoryResponse>? _subcategories = [];


    protected override async Task OnInitializedAsync()
    {
        try
        {
            _contacts = await _httpClient.GetFromJsonAsync<List<GetContactResponse>>(
                "http://host.docker.internal:8080/contacts");
            _mainCategories = await _httpClient.GetFromJsonAsync<List<GetCategoryResponse>>(
                "http://host.docker.internal:8080/contacts/categories?mainCategories=true");
            _subcategories = await _httpClient.GetFromJsonAsync<List<GetCategoryResponse>>(
                "http://host.docker.internal:8080/contacts/categories?mainCategories=false");
        }
        catch (HttpRequestException)
        {
            NavigationManager.NavigateTo("/Error");
        }
    }
}