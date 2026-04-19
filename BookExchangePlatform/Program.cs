using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookExchangeDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options =>
{
options.SignIn.RequireConfirmedAccount = false;
options.SignIn.RequireConfirmedEmail = false;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<BookExchangeDbContext>();
builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();


builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IReviewService, ReviewService>();


var app = builder.Build();





using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed roles
    string[] roles = { "Administrator", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Seed admin user
    if (await userManager.FindByEmailAsync("admin@admin.com") == null)
    {
        var admin = new User
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(admin, "Admin1234");
        await userManager.AddToRoleAsync(admin, "Administrator");
    }

    // Seed regular user
    if (await userManager.FindByEmailAsync("user@user.com") == null)
    {
        var user = new User
        {
            UserName = "user@user.com",
            Email = "user@user.com",
            FirstName = "Regular",
            LastName = "User",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user, "User1234");
        await userManager.AddToRoleAsync(user, "User");
    }

    var db = services.GetRequiredService<BookExchangeDbContext>();
    var seedUser = await userManager.FindByEmailAsync("user@user.com");

    if (!db.Books.Any())
    {
        db.Books.AddRange(
            new Book { Title = "1984", Author = "George Orwell", Description = "A dystopian novel.", Genre = "Dystopia", Condition = "Good", DateOfPublishing = new DateTime(1949, 6, 8), OwnerId = seedUser!.Id },
            new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Description = "A fantasy adventure.", Genre = "Fantasy", Condition = "Good", DateOfPublishing = new DateTime(1937, 9, 21), OwnerId = seedUser!.Id },
            new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Description = "A story of justice.", Genre = "Drama", Condition = "Good", DateOfPublishing = new DateTime(1960, 7, 11), OwnerId = seedUser!.Id }
        );
        await db.SaveChangesAsync();
    }

    if (!db.Movies.Any())
    {
        db.Movies.AddRange(
            new Movie { Title = "Inception", Director = "Christopher Nolan", ReleaseYear = new DateTime(2010, 7, 16), Genre = "Sci-Fi", Resume = "A thief enters dreams to steal secrets.", OwnerId = seedUser!.Id },
            new Movie { Title = "The Godfather", Director = "Francis Ford Coppola", ReleaseYear = new DateTime(1972, 3, 24), Genre = "Crime", Resume = "A mafia family drama.", OwnerId = seedUser!.Id },
            new Movie { Title = "Interstellar", Director = "Christopher Nolan", ReleaseYear = new DateTime(2014, 11, 7), Genre = "Sci-Fi", Resume = "Astronauts search for a new home for humanity.", OwnerId = seedUser!.Id }
        );
        await db.SaveChangesAsync();
    }
}


app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

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
    name: "Admin",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();




app.Run();