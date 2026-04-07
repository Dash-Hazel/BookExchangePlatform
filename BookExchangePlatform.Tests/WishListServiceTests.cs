using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Tests;

public class WishListServiceTests : IDisposable
{
    private readonly BookExchangeDbContext currContext;

    private readonly WishListService wishListService;

    public WishListServiceTests()
    {
        var options = new DbContextOptionsBuilder<BookExchangeDbContext>()

            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

            .Options;

        currContext = new BookExchangeDbContext(options);

        wishListService = new WishListService(currContext);
    }


    public void Dispose()
    {
        currContext.Database.EnsureDeleted();

        currContext.Dispose();
    }

    [Fact]
    public async Task AddItemToWishListAsync_AddsItem()
    {
        // Arrange
        var user = new User { Id = "user1", FirstName = "Test", LastName = "User", Email = "test@test.com" };
        currContext.Users.Add(user);
        await currContext.SaveChangesAsync();

        // Act
        var result = await wishListService.AddItemToWishListAsync("user1", 1, null);

         // Assert
                Assert.NotNull(result);
        Assert.Equal("user1", result.UserId);
            Assert.Equal(1, result.BookId);
    }

    [Fact]
    public async Task RemoveFromWishListAsync_RemovesItem()
    {
        // Arrange
        var user = new User { Id = "user2", FirstName = "Test", LastName = "User", Email = "test@test.com" };


        currContext.Users.Add(user);
        await currContext.SaveChangesAsync();

                 var wishlist = await wishListService.AddItemToWishListAsync("user2", 2, null);
            var wishlistId = wishlist.Id;

        // Act
        var result = await wishListService.RemoveFromWishListAsync(wishlistId);


        // Assert
        Assert.True(result);
             var deleted = await currContext.WishLists.FindAsync(wishlistId);
         Assert.Null(deleted);
    }

    [Fact]
    public async Task ExistsInWishList_ChecksIfItemExists()
    {
        // Arrange
        var user = new User { Id = "user3", FirstName = "Test", LastName = "User", Email = "test@test.com" };


        currContext.Users.Add(user);

        await currContext.SaveChangesAsync();



        await wishListService.AddItemToWishListAsync("user3", 3, null);

        // Act

             var exists = await wishListService.ExistsInWishList("user3", 3, null);
        var notExists = await wishListService.ExistsInWishList("user3", 99, null);  


        // Assert
         Assert.True(exists);
             Assert.False(notExists);
    }
}