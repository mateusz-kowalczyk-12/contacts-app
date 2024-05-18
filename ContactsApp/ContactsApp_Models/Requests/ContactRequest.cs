using System.ComponentModel.DataAnnotations;

namespace ContactsApp_Models.Requests;

public class ContactRequest
{
    [MaxLength(256)]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(256)]
    public string LastName { get; set; } = null!;
    
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    
    [MinLength(8)]
    [MaxLength(256)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$")]
    public string Password { get; set; } = null!;
    
    public Guid MainCategoryId { get; set; }
    
    public Guid? BusinessSubcategoryId { get; set; }
    
    [MaxLength(256)]
    public string? OtherSubcategory { get; set; }
    
    [MaxLength(256)]
    public string PhoneNumber { get; set; } = null!;
    
    public int BirthDateYear { get; set; }
    
    public int BirthDateMonth { get; set; }
    
    public int BirthDateDay { get; set; }
}