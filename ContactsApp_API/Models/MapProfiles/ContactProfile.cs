using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ContactsApp_API.Data;
using ContactsApp_API.Data.ContactsContext;
using ContactsApp_API.Models.Requests;
using ContactsApp_API.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace ContactsApp_API.Models.MapProfiles;

public class ContactProfile: Profile
{
    public ContactProfile()
    {
        CreateMap<ContactRequest, Contact>()
            .ForMember(
                dest => dest.FirstName,
                options =>
                    options.MapFrom(src => src.FirstName))
            .ForMember(
                dest => dest.LastName,
                options =>
                    options.MapFrom(src => src.LastName))
            .ForMember(
                dest => dest.Email,
                options =>
                    options.MapFrom(src => src.Email))
            .ForMember(
                dest => dest.PasswordHash,
                options =>
                    options.MapFrom(src =>
                        Convert.ToBase64String(
                            SHA256.HashData(Encoding.UTF8.GetBytes(src.Password)))))
            .ForMember(
                dest => dest.MainCategory,
                options =>
                    options.MapFrom<CategoryResolver>())
            .ForMember(
                dest => dest.BusinessSubcategory,
                options =>
                    options.MapFrom<BusinessSubcategoryResolver>())
            .ForMember(
                dest => dest.OtherSubcategory,
                options =>
                    options.MapFrom(src => src.OtherSubcategory))
            .ForMember(
                dest => dest.BusinessSubcategory,
                options =>
                    options.MapFrom<BusinessSubcategoryResolver>())
            .ForMember(
                dest => dest.PhoneNumber,
                options =>
                    options.MapFrom(src => src.PhoneNumber))
            .ForMember(
                dest => dest.BirthDate,
                options =>
                    options.MapFrom(src =>
                        new DateOnly(src.BirthDateYear, src.BirthDateMonth, src.BirthDateDay)));

        CreateMap<Contact, GetContactResponse>()
            .ForMember(
                dest => dest.Id,
                options =>
                    options.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.FirstName,
                options =>
                    options.MapFrom(src => src.FirstName))
            .ForMember(
                dest => dest.LastName,
                options =>
                    options.MapFrom(src => src.LastName))
            .ForMember(
                dest => dest.Email,
                options =>
                    options.MapFrom(src => src.Email))
            .ForMember(
                dest => dest.PasswordHash,
                options =>
                    options.MapFrom(src => src.PasswordHash.ToString()))
            .ForMember(
                dest => dest.MainCategoryId,
                options =>
                    options.MapFrom(src => src.MainCategory.Id))
            .ForMember(
                dest => dest.BusinessSubcategoryId,
                options =>
                    options.MapFrom(src =>
                        src.BusinessSubcategory != null ? src.BusinessSubcategory.Id : (Guid?)null))
            .ForMember(
                dest => dest.OtherSubcategory,
                options =>
                    options.MapFrom(src => src.OtherSubcategory))
            .ForMember(
                dest => dest.PhoneNumber,
                options =>
                    options.MapFrom(src => src.PhoneNumber))
            .ForMember(
                dest => dest.BirthDateYear,
                options =>
                    options.MapFrom(src => src.BirthDate.Year))
            .ForMember(
                dest => dest.BirthDateMonth,
                options =>
                    options.MapFrom(src => src.BirthDate.Month))
            .ForMember(
                dest => dest.BirthDateDay,
                options =>
                    options.MapFrom(src => src.BirthDate.Day));
    }


    private class CategoryResolver(ContactsDbContext contactsDbContext)
        : IValueResolver<ContactRequest, Contact, Category>
    {
        public Category Resolve(
            ContactRequest src, Contact dest, Category category, ResolutionContext context)
        {
            return contactsDbContext.Categories.Find(src.MainCategoryId) ?? throw new InvalidOperationException();
        }
    }
    
    private class BusinessSubcategoryResolver(ContactsDbContext contactsDbContext)
        : IValueResolver<ContactRequest, Contact, Category?>
    {
        public Category? Resolve(
            ContactRequest src, Contact dest, Category? category, ResolutionContext context)
        {
            return contactsDbContext.Categories.Find(src.BusinessSubcategoryId);
        }
    }
}