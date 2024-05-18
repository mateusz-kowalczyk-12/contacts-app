namespace ContactsApp_Models.Responses;

public class GetCategoryResponse
{
    public Guid Id { get; set; }
    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
}