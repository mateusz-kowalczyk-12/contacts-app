using AutoMapper;
using ContactsApp_API.Data;
using ContactsApp_API.Data.ContactsContext;
using ContactsApp_API.Models.Responses;

namespace ContactsApp_API.Models.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, GetCategoryResponse>();
    }
}