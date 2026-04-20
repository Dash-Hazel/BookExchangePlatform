using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Tests
{
    public class BookServiceTests : IDisposable
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
            // Arrange
            var book = new Book
            {
                Title = "Surrounded by idiots",
                Description = "Description of a Book",
                Author = "Thomas Erickson",
                Genre = "Comedy",
                OwnerId = "1"
            };

            currContext.Books.Add(book);
            await currContext.SaveChangesAsync();

            var savedBookId = book.Id;

            // Act
            var result = await bookkService.GetBookByIdAsync(savedBookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Surrounded by idiots", result.Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await bookkService.GetBookByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateBookAsync_WithValidBook_SavesToDatabase()
        {
            // Arrange
            var book = new Book
            {
                Title = "New Book",
                Author = "Some Author",
                OwnerId = "1",
                Description = "Description of the book",
                Genre = "Horror"
            };

            // Act
            var result = await bookkService.CreateBookAsync(book);

            // Assert
            Assert.NotNull(result);
            var savedBook = await currContext.Books.FirstOrDefaultAsync(b => b.Title == "New Book");
            Assert.NotNull(savedBook);
        }

        [Fact]
        public async Task UpdateBookAsync_WithValidId_UpdatesBook()
        {
            // Arrange
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

            // Act
            var result = await bookkService.UpdateBookAsync(book.Id, updatedBook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var updatedBook = new Book { Title = "Updated", Author = "Author", OwnerId = "1" };

            // Act
            var result = await bookkService.UpdateBookAsync(999, updatedBook);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidId_DeletesBook()
        {
            // Arrange
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

            // Act
            var result = await bookkService.DeleteBookAsync(bookId);

            // Assert
            Assert.True(result);
            var deletedBook = await currContext.Books.FindAsync(bookId);
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task DeleteBookAsync_WithInvalidId_ReturnsFalse()
        {
            // Act
            var result = await bookkService.DeleteBookAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            currContext.Users.Add(user);
            await currContext.SaveChangesAsync();

            currContext.Books.AddRange(
                new Book { Title = "Book 1", Author = "Author 1", Description = "Desc", Genre = "Genre", OwnerId = "1" },
                new Book { Title = "Book 2", Author = "Author 2", Description = "Desc", Genre = "Genre", OwnerId = "1" }
            );

            await currContext.SaveChangesAsync();

            // Act
            var result = await bookkService.GetAllBooksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllBooksAsync_WithSearch_ReturnsFilteredBooks()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            currContext.Users.Add(user);
            await currContext.SaveChangesAsync();

            currContext.Books.AddRange(
                new Book { Title = "Harry Potter", Author = "Rowling", Description = "Desc", Genre = "Fantasy", OwnerId = "1" },
                new Book { Title = "The Hobbit", Author = "Tolkien", Description = "Desc", Genre = "Fantasy", OwnerId = "1" }
            );

            await currContext.SaveChangesAsync();

            // Act
            var result = await bookkService.GetAllBooksAsync(search: "Harry");

            // Assert
            Assert.Single(result);
            Assert.Equal("Harry Potter", result[0].Title);
        }

        [Fact]
        public async Task GetAllBooksAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            currContext.Users.Add(user);
            await currContext.SaveChangesAsync();

            for (int i = 1; i <= 15; i++)
            {
                currContext.Books.Add(new Book { Title = $"Book {i}", Author = "Author", Description = "Desc", Genre = "Genre", OwnerId = "1" });
            }

            await currContext.SaveChangesAsync();

            // Act
            var result = await bookkService.GetAllBooksAsync(page: 2, pageSize: 10);

            // Assert
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task GetBookWithOwnerAsync_WhenBookExists_ReturnsBookWithOwner()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                UserName = "test@test.com",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            currContext.Users.Add(user);

            var book = new Book { Title = "Test Book", Author = "Author", Description = "Desc", Genre = "Genre", OwnerId = "user1" };

            currContext.Books.Add(book);
            await currContext.SaveChangesAsync();

            // Act
            var result = await bookkService.GetBookWithOwnerAsync(book.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Owner);
        }

        [Fact]
        public async Task GetBookWithOwnerAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await bookkService.GetBookWithOwnerAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            currContext.Users.AddRange(
                new User { Id = "1", UserName = "user1@test.com", Email = "user1@test.com", FirstName = "User", LastName = "One" },
                new User { Id = "2", UserName = "user2@test.com", Email = "user2@test.com", FirstName = "User", LastName = "Two" }
            );

            await currContext.SaveChangesAsync();

            // Act
            var result = await bookkService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetFirstUserAsync_WhenUsersExist_ReturnsUser()
        {
            // Arrange
            currContext.Users.Add(new User { Id = "1", UserName = "user@test.com", Email = "user@test.com", FirstName = "User", LastName = "One" });
            await currContext.SaveChangesAsync();

            // Act
            var result = await bookkService.GetFirstUserAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}
