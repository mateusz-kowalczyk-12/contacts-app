using ContactsApp_Models.Responses;

namespace ContactsApp_Web.States;

public class AppState
{
    public List<GetContactResponse>? Contacts { get; set; } = [];
    public List<GetCategoryResponse>? MainCategories { get; set; } = [];
    public List<GetCategoryResponse>? BusinessSubcategories { get; set; } = [];
    
    public GetContactResponse? SelectedContact { get; set; }
}