using ContactsApp_API.Data;
using ContactsApp_API.Data.ContactsContext;
using ContactsApp_API.Data.UsersContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

using CategoryLevel = ContactsApp_API.Data.ContactsContext.Category.CategoryLevel;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding JSON serializer
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    )
    .AddNewtonsoftJson(options => 
        options.SerializerSettings.ContractResolver = new DefaultContractResolver()
    );

// Adding authentication & authorization
builder.Services
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

// Adding Identity Core configuration
builder.Services
    .AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddApiEndpoints();

// Adding the database contexts
builder.Services.AddDbContext<ContactsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsAppDB")));
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsAppDB")));

// Add AutoMapper for mapping between DTOs and entities
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Enabling CORS
app.UseCors(policyBuilder =>
    policyBuilder
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod());

// Adding Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapping controllers in to API endpoints
app.MapControllers();
// Mapping Identity Core endpoints
app.MapIdentityApi<IdentityUser>();

// TODO: Add HTTPS
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    // Updating the database with pending migrations
    var contactsDbContext = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
    var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    
    if (contactsDbContext.Database.GetPendingMigrations().Any())
        contactsDbContext.Database.Migrate();
    if (usersDbContext.Database.GetPendingMigrations().Any())
        usersDbContext.Database.Migrate();
    
    // Seeding the database with the main categories and subcategories
    if (!contactsDbContext.Categories.Any())
    {
        var categories = new List<Category>
        {
            new() { Key = "business", Name = "służbowy", Level = CategoryLevel.MainCategory },
            new() { Key = "private", Name = "prywatny", Level = CategoryLevel.MainCategory },
            new() { Key = "other", Name = "inny", Level = CategoryLevel.MainCategory },
            new() { Key = "chairman", Name = "prezes", Level = CategoryLevel.Subcategory },
            new() { Key = "manager", Name = "menedżer", Level = CategoryLevel.Subcategory },
            new() { Key = "coworker", Name = "współpracownik", Level = CategoryLevel.Subcategory },
            new() { Key = "client", Name = "klient", Level = CategoryLevel.Subcategory }
        };
        foreach (var category in categories)
        {
            await contactsDbContext.Categories.AddAsync(category);
        }

        await contactsDbContext.SaveChangesAsync();
    }
}

app.Run();