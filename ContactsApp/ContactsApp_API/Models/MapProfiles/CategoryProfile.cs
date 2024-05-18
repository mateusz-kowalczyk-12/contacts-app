using AutoMapper;
using ContactsApp_API.Data.ContactsContext;
using ContactsApp_Models.Responses;

namespace ContactsApp_API.Models.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, GetCategoryResponse>();
    }
}