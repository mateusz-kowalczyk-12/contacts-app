using System.Text;
using System.Text.Json;
using ContactsApp_Models.Requests;
using ContactsApp_Models.Responses;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Contacts;

public partial class NewContact
{
    [Inject] private AppState AppState { get; set; } = null!;
    private readonly HttpClient _httpClient = new();
    
    private ContactRequest _newContact = new();
    private DateOnly _newContactBirthDate = DateOnly.FromDateTime(DateTime.Today);
    
    private bool _isError = false;
    private bool _isSuccess = false;


    protected override void OnInitialized()
    {
        _httpClient.BaseAddress = new Uri("http://host.docker.internal:8080/");
    }


    private async Task AddNewContact()
    {
        // Reset the request error
        _isError = false;

        // Add the date to the model since it has not been bound to any input
        _newContact.BirthDateYear = _newContactBirthDate.Year;
        _newContact.BirthDateMonth = _newContactBirthDate.Month;
        _newContact.BirthDateDay = _newContactBirthDate.Day;
        
        var response = await _httpClient.PostAsJsonAsync("contacts", _newContact);
        
        if (response.IsSuccessStatusCode)
            _isSuccess = true;
        else
            _isError = true;
    }

    private string? GetNewContactMainCategoryKey()
    {
        var mainCategory = AppState.MainCategories?
            .Where(category => category.Id == _newContact.MainCategoryId)
            .FirstOrDefault();

        return mainCategory?.Key;
    }

    private void DeleteSubcategory(string mainCategoryKey)
    {
        switch (mainCategoryKey)
        {
            case "other":
                _newContact.OtherSubcategory = null;
                break;
            case "business":
                _newContact.BusinessSubcategoryId = null;
                break;
        }
    }
}