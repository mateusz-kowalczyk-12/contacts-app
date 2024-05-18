using AutoMapper;
using ContactsApp_API.Data.ContactsContext;
using ContactsApp_Models.Requests;
using ContactsApp_Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CategoryLevel = ContactsApp_API.Data.ContactsContext.Category.CategoryLevel;

namespace ContactsApp_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContactsController(ContactsDbContext contactsDbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetContactById([FromRoute] Guid id)
        {
            var contact = await contactsDbContext.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();
            
            return Ok(contact);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await contactsDbContext.Contacts
                .Include(contact => contact.MainCategory)
                .Include(contact => contact.BusinessSubcategory)
                .Select(contact => mapper.Map<GetContactResponse>(contact))
                .ToListAsync();
            
            return Ok(contacts);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] bool mainCategories)
        {
            var categories = await contactsDbContext.Categories
                .Where(category =>
                    category.Level == (mainCategories ? CategoryLevel.MainCategory : CategoryLevel.Subcategory))
                .Select(category => mapper.Map<GetCategoryResponse>(category))
                .ToListAsync();

            return Ok(categories);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactRequest contactRequest)
        {
            // Validate
            var errorMessage = await ValidateContactRequestAsync(contactRequest, true);
            if (errorMessage != null)
                return BadRequest(errorMessage);

            // Create and add new contact
            var newContact = mapper.Map<Contact>(contactRequest);
            await contactsDbContext.Contacts.AddAsync(newContact);
            await contactsDbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetContactById),
                new { id = newContact.Id },
                mapper.Map<GetContactResponse>(newContact));
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, [FromBody] ContactRequest contactRequest)
        {
            var originalContact = await contactsDbContext.Contacts
                .Include(contact => contact.MainCategory)
                .Include(contact => contact.BusinessSubcategory)
                .FirstOrDefaultAsync(contact => contact.Id == id);
            if (originalContact == null)
                return NotFound();
            
            // Store the information whether a user requested the contact's email to be changed
            var requestedEmailUpdate = originalContact.Email != contactRequest.Email;
            
            // Validate the DTO
            var errorMessage = await ValidateContactRequestAsync(contactRequest, requestedEmailUpdate);
            if (errorMessage != null)
                return BadRequest(errorMessage);
            
            // Map the DTO into a contact with the new values
            var updatedContact = mapper.Map<Contact>(contactRequest);
            // Do not change the primary key
            updatedContact.Id = originalContact.Id;
            
            // Update the original contact with the new values
            contactsDbContext.Entry(originalContact).CurrentValues.SetValues(updatedContact);
            // Also update the foreign keys
            originalContact.MainCategory = updatedContact.MainCategory;
            originalContact.BusinessSubcategory = updatedContact.BusinessSubcategory;
            
            // Save the changes
            contactsDbContext.Contacts.Update(originalContact);
            await contactsDbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await contactsDbContext.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();
            
            contactsDbContext.Contacts.Remove(contact);
            await contactsDbContext.SaveChangesAsync();

            return NoContent();
        }


        private async Task<string?> ValidateContactRequestAsync(ContactRequest contactRequest, bool checkEmailUniqueness)
        {
            if (checkEmailUniqueness)
                // The request requires that the given email does not already exist in the contacts table
            {
                // Check if the email is unique
                var emailAlreadyExists = await contactsDbContext.Contacts
                    .Where(contact => contact.Email == contactRequest.Email)
                    .AnyAsync();
                if (emailAlreadyExists)
                    return "Email already exists";
            }
            
            // Check if the main category exists
            var mainCategory =
                await contactsDbContext.Categories.FindAsync(contactRequest.MainCategoryId);
            if (mainCategory is not { Level: CategoryLevel.MainCategory })
                return "Main category does not exist";
            
            if (mainCategory.Key == "private")
            {
                // Check if a private category does not have a subcategory selected 
                if (contactRequest.BusinessSubcategoryId != null || contactRequest.OtherSubcategory != null)
                    return "Private category cannot have a subcategory";
            }
            else if (mainCategory.Key == "business")
            {
                // Check if the business category does not have an other subcategory
                if (contactRequest.OtherSubcategory != null)
                    return "Business category cannot have an other subcategory";
                
                // If a business subcategory is selected, check if it exists
                if (contactRequest.BusinessSubcategoryId != null)
                {
                    var businessSubcategory =
                        await contactsDbContext.Categories.FindAsync(contactRequest.BusinessSubcategoryId);
                    if (businessSubcategory is not { Level: CategoryLevel.Subcategory })
                        return "Subcategory does not exist";
                }
            }
            else if (mainCategory.Key == "other")
            {
                // Check if the other category does not have a business subcategory
                if (contactRequest.BusinessSubcategoryId != null)
                    return "Other category cannot have a business subcategory";
            }
            
            // Check if the given birth date is in range of the DateOnly struct
            try
            {
                _ = new DateOnly(
                    contactRequest.BirthDateYear,
                    contactRequest.BirthDateMonth,
                    contactRequest.BirthDateDay);
            }
            catch (ArgumentOutOfRangeException)
            {
                return "Birth date out of range";
            }

            return null;
        }
    }
}
