using BookExchangePlatform.Data;
using BookExchangePlatform.Services;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookExchangeDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<BookExchangePlatform.Models.User>(options => options.SignIn.RequireConfirmedAccount = true)
     .AddEntityFrameworkStores<BookExchangeDbContext>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookExchangeDbContext>();


    // Seed
    if (!dbContext.Users.Any())
    {
        dbContext.Users.Add(new BookExchangePlatform.Models.User
        {
            FirstName = "Johnson",
            LastName = "McCall",
            Email = "test@test.com",
            PhoneNumber = "1234567890",
        });
        dbContext.SaveChanges();
        Console.WriteLine("Seed user created with ID: 1");
    }
}




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages(); 

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();