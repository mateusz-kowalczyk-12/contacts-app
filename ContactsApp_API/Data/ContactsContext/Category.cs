using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp_API.Data.ContactsContext;

public class Category
{
    public enum CategoryLevel
    {
        MainCategory,
        Subcategory
    }
    
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [MaxLength(256)]
    public string Key { get; set; } = null!;
    
    [MaxLength(256)]
    public string Name { get; set; } = null!;

    public CategoryLevel Level { get; set; }
}