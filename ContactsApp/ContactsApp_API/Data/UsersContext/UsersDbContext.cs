using ContactsApp_API.Data.ContactsContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp_API.Data.UsersContext;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext<IdentityUser>(options);