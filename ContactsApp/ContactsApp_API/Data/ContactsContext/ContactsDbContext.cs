using Microsoft.EntityFrameworkCore;

namespace ContactsApp_API.Data.ContactsContext;

public class ContactsDbContext(DbContextOptions<ContactsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Contact>()
            .HasIndex(contact => contact.Email)
            .IsUnique();
    }
}