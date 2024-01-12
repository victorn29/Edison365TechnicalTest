using Edison365TechnicalTest.Data;
using Edison365TechnicalTest.Entities;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddControllers().AddOData(options => options.AddRouteComponents(
    GetEdmModel()).
    Expand());

// Seed the database with test data
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData(context);
}

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Book>("Books");
    builder.EntitySet<Author>("Authors");

    return builder.GetEdmModel();
}
static void SeedData(AppDbContext context)
{
    // Add test data for Books
    var books = new List<Book>
    {
        new() { Name = "The Lord of the rings" },
        new() { Name = "Harry Potter" },
        new() { Name = "The Pillars of the Earth" }
    };
    context.Books.AddRange(books);
    context.SaveChanges();

    // Add test data for Authors
    var authors = new List<Author>
    {
        new() { FirstName = "J.R.R", LastName = "Tolkien" },
        new() { FirstName = "J.K", LastName = "Rowling" },
        new() { FirstName = "Ken", LastName = "Follet" }
    };
    context.Authors.AddRange(authors);
    context.SaveChanges();

    // Add test data for BookAuthors
    var bookAuthors = new List<BookAuthor>
    {
        new() { BookID = 1, AuthorID = 1 },
        new() { BookID = 2, AuthorID = 2 },
        new() { BookID = 3, AuthorID = 3 }
    };
    context.BookAuthors.AddRange(bookAuthors);
    context.SaveChanges();
}
