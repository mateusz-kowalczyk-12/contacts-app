using ContactsApp_API.Data;

namespace ContactsApp_API.Models.Responses;

public class GetContactResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Guid MainCategoryId { get; set; }
    public Guid? BusinessSubcategoryId { get; set; }
    public string? OtherSubcategory { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public int BirthDateYear { get; set; }
    public int BirthDateMonth { get; set; }
    public int BirthDateDay { get; set; }
}