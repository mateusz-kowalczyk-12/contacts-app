using ContactsApp_Models.Requests;
using ContactsApp_Models.Responses;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Components;

namespace ContactsApp_Web.Components.Contacts;

public partial class NewContact
{
    [Inject] private AppState AppState { get; set; } = null!;
    private ContactRequest _newContact = new();
    private DateOnly _newContactBirthDate = new();


    private async Task AddNewContact()
    {
        // Add the date to the model since it has not been bound to any input
        _newContact.BirthDateYear = _newContactBirthDate.Year;
        _newContact.BirthDateMonth = _newContactBirthDate.Month;
        _newContact.BirthDateDay = _newContactBirthDate.Day;
        
        var a = 1;
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