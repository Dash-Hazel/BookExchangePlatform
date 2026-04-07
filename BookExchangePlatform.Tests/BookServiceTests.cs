using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace BookExchangePlatform.Tests;

public class BookServiceTests: IDisposable
{
    private readonly BookExchangeDbContext currContext;
    private readonly BookService bookkService;

    public BookServiceTests()
    {

        var options = new DbContextOptionsBuilder<BookExchangeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        currContext = new BookExchangeDbContext(options);
        bookkService = new BookService(currContext);
    }

    public void Dispose()
    {
        currContext.Database.EnsureDeleted();
        currContext.Dispose();
    }

    [Fact]
    public async Task GetBookByIdAsync_WhenBookExists_ShouldReturnThatBook()
    {
        //Arrange
        var book = new Book{ Title = "Surrounded by idiots", Description = "Description of a Book", Author = "Thomas Erickson", Genre = "Comedy", OwnerId = "1" };
        currContext.Books.Add(book);
        await currContext.SaveChangesAsync();

        var savedBookId = book.Id;
        //Act
        var result = await bookkService.GetBookByIdAsync(savedBookId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Surrounded by idiots", result.Title);
    }

    [Fact]
    public async Task GetBookByIdAsync_WithInvalidId_ReturnsNull()
    {
        // ACT
        var result = await bookkService.GetBookByIdAsync(999);

        // ASSERT
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateBookAsync_WithValidBook_SavesToDatabase()
    {
        //Arrange

        var book = new Book
        {
            Title = "New Book",
            Author = "Some Author",
            OwnerId = "1",
            Description = "Description of the book",
            Genre = "Horror"
            
        };

        // ACT
        var result = await bookkService.CreateBookAsync(book);

        // ASSERT
        Assert.NotNull(result);
        var savedBook = await currContext.Books.FirstOrDefaultAsync(b => b.Title == "New Book");
        Assert.NotNull(savedBook);
    }

    [Fact]
    public async Task UpdateBookAsync_WithValidId_UpdatesBook()
    {
        //Arrange

        var book = new Book
        {
            Title = "Original Book",
            Author = "Author",
            OwnerId = "1",
            Description = "Description of the book",
            Genre = "Horror"
        };
        currContext.Books.Add(book);
        await currContext.SaveChangesAsync();

        var updatedBook = new Book
        {
            Title = "New Title",
            Author = "Author",
            OwnerId = "1",
            Description = "Description of the book",
            Genre = "Horror"
        };


        // ACT
        var result = await bookkService.UpdateBookAsync(book.Id, updatedBook);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("New Title", result.Title);
    }

    [Fact]
    public async Task UpdateBookAsync_WithInvalidId_ReturnsNull()
    {
        // ARRANGE
        var updatedBook = new Book { Title = "Updated", Author = "Author", OwnerId = "1" };

        // ACT
        var result = await bookkService.UpdateBookAsync(999, updatedBook);

        // ASSERT
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteBookAsync_WithValidId_DeletesBook()
    {
        // ARRANGE
        var book = new Book
        {
            Title = "Delete",
            Author = "Author",
            OwnerId = "1",
            Description = "Description of the book",
            Genre = "Horror"
        };

        currContext.Books.Add(book);
        await currContext.SaveChangesAsync();
        var bookId = book.Id;


        // ACT
        var result = await bookkService.DeleteBookAsync(bookId);

        // ASSERT
        Assert.True(result);
        var deletedBook = await currContext.Books.FindAsync(bookId);
        Assert.Null(deletedBook);
    }


    [Fact]
    public async Task DeleteBookAsync_WithInvalidId_ReturnsFalse()
    {
        // ACT
        var result = await bookkService.DeleteBookAsync(999);

        // ASSERT
        Assert.False(result);
    }
}
