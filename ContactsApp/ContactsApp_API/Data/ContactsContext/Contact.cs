using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp_API.Data.ContactsContext;

public class Contact
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [MaxLength(256)]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(256)]
    public string LastName { get; set; } = null!;
    
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    
    [MaxLength(256)]
    public string Password { get; set; } = null!;
    
    public Category MainCategory { get; set; } = null!;
    
    public Category? BusinessSubcategory { get; set; }
    
    [MaxLength(256)]
    public string? OtherSubcategory { get; set; }
    
    [MaxLength(256)]
    public string PhoneNumber { get; set; } = null!;
    
    public DateOnly BirthDate { get; set; }
}