using BookExchangePlatform.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookExchangeDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookExchangeDbContext>();

 
    dbContext.Database.EnsureCreated();

    // Seed
    if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(new BookExchangePlatform.Models.User
        {
            FirstName = "Johnson",
            LastName = "McCall",
            Email = "test@test.com",
            PhoneNumber = "1234567890",
            Location = "Test City"
        });
        dbContext.SaveChanges();
        Console.WriteLine("Seed user created with ID: 1");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
