using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using ContactsApp_Models.Requests;
using ContactsApp_Models.Responses;
using ContactsApp_Web.GlobalHelpers;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Contacts;

public partial class EditableContact
{
    [Inject] private AppState AppState { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;

    
    [Parameter] public bool IsNewContact { get; set; }
    
    private readonly HttpClient _httpClient = new();
    
    private ContactRequest _contactModel = null!;
    private DateOnly _newContactBirthDate;
    
    private bool _isError = false;
    private bool _isSuccess = false;


    protected override void OnInitialized()
    {
        _httpClient.BaseAddress = new Uri("http://host.docker.internal:8080/");
        
        // Set the birthdate input initial value to an appropriate date
        if (IsNewContact)
            // if a new contact is to be added
        {
            _newContactBirthDate = DateOnly.FromDateTime(DateTime.Today);
            _contactModel = new();
        }
        else
            // if a contact is to be edited
        {
            if (AppState.SelectedContact != null)
                // if a contact to be edit is selected
            {
                _newContactBirthDate = new DateOnly(
                    AppState.SelectedContact.BirthDateYear,
                    AppState.SelectedContact.BirthDateMonth,
                    AppState.SelectedContact.BirthDateDay);

                _contactModel = new ContactRequest
                {
                    FirstName = AppState.SelectedContact.FirstName,
                    LastName = AppState.SelectedContact.LastName,
                    Email = AppState.SelectedContact.Email,
                    Password = AppState.SelectedContact.Password,
                    MainCategoryId = AppState.SelectedContact.MainCategoryId,
                    BusinessSubcategoryId = AppState.SelectedContact.BusinessSubcategoryId,
                    OtherSubcategory = AppState.SelectedContact.OtherSubcategory,
                    PhoneNumber = AppState.SelectedContact.PhoneNumber,
                    BirthDateYear = AppState.SelectedContact.BirthDateYear,
                    BirthDateMonth = AppState.SelectedContact.BirthDateMonth,
                    BirthDateDay = AppState.SelectedContact.BirthDateDay,
                };
            }
            else
                // if the contact to be edited is not selected (this is an error)
            {
                _contactModel = new ContactRequest();
            }
        }
    }


    private async Task AddNewContact()
    {
        // Reset the request error
        _isError = false;
        
        // Add the date to the model (DTO) since it the date has not been bound to any input
        AddBirthDateToModel();
        
        // Add the Bearer token to the request
        if (!await AccessTokenHelper.LoadBearerTokenAsync(LocalStorage, _httpClient))
            // if the token was not found
        {
            _isError = true;
            return;
        }
        
        var response = await _httpClient.PostAsJsonAsync("contacts", _contactModel);
        CheckResponse(response);
    }

    private async Task EditContact()
    {
        if (AppState.SelectedContact == null)
        {
            _isError = true;
            return;
        }
            
        // Reset the request error and succes
        _isError = false;
        _isSuccess = false;
        
        // Add the date to the DTO since it the date has not been bound to any input
        AddBirthDateToModel();
        
        // Add the Bearer token to the request
        if (!await AccessTokenHelper.LoadBearerTokenAsync(LocalStorage, _httpClient))
            // if the token was not found
        {
            _isError = true;
            return;
        }

        var response =
            await _httpClient.PutAsJsonAsync($"contacts/{AppState.SelectedContact.Id}", _contactModel);
        CheckResponse(response);
    }

    
    private string? GetNewContactMainCategoryKey()
    {
        var mainCategory = AppState.MainCategories?
            .Where(category => category.Id == _contactModel.MainCategoryId)
            .FirstOrDefault();

        return mainCategory?.Key;
    }

    private void DeleteSubcategory(string mainCategoryKey)
    {
        switch (mainCategoryKey)
        {
            case "other":
                _contactModel.OtherSubcategory = null;
                break;
            case "business":
                _contactModel.BusinessSubcategoryId = null;
                break;
        }
    }
    
    private void AddBirthDateToModel()
    {
        _contactModel.BirthDateYear = _newContactBirthDate.Year;
        _contactModel.BirthDateMonth = _newContactBirthDate.Month;
        _contactModel.BirthDateDay = _newContactBirthDate.Day;
    }

    private void CheckResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            _isSuccess = true;
            _isError = false;
        }
        else
        {
            _isError = true;
            _isSuccess = false;
        }
    }
}