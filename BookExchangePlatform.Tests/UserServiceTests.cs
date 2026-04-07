using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookExchangePlatform.Tests
{
    public class UserServiceTests: IDisposable
    {
        private readonly BookExchangeDbContext currContext;
        private readonly UserService userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookExchangeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            currContext = new BookExchangeDbContext(options);

            userService = new UserService(currContext);
        }

        public void Dispose()
        {
            currContext.Database.EnsureDeleted();
            currContext.Dispose();
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = "user123",
                FirstName = "John",
                LastName = "Doe",

                Email = "john@example.com"
            };
            currContext.Users.Add(user);

            await currContext.SaveChangesAsync();

            // Act
            var result = await userService.GetUserByIdAsync("user123");

            // Assert
            Assert.NotNull(result);


            Assert.Equal("John", result.FirstName);

            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await userService.GetUserByIdAsync("nonexistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithNullId_ReturnsNull()
        {
            // Act
            var result = await userService.GetUserByIdAsync(null);

            // Assert
                    Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_SavesUser()
        {
            // Arrange
            var user = new User
            {
                Id = "newuser",
                 FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com"
            };

            // Act
            var result = await userService.CreateUserAsync(user);

            // Assert
            Assert.NotNull(result);

            var savedUser = await currContext.Users.FirstOrDefaultAsync(u => u.Id == "newuser");
            Assert.NotNull(savedUser);


            Assert.Equal("Jane", savedUser.FirstName);
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidId_UpdatesUser()
        {
            // Arrange
            var user = new User
            {
                Id = "user456",

                FirstName = "Old",
                LastName = "Name",
                  Email = "old@example.com"
            };
            currContext.Users.Add(user);
            await currContext.SaveChangesAsync();

            var updatedInfo = new User
            {
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com"
            };

            // Act
            var result = await userService.UpdateUserAsync("user456", updatedInfo);

            // Assert
                Assert.NotNull(result); 
            Assert.Equal("New", result.FirstName);
            Assert.Equal("new@example.com", result.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var updatedInfo = new User
            {
                FirstName = "Any",
                LastName = "Name",
                Email = "any@example.com"
            };

            // Act
            var result = await userService.UpdateUserAsync("nonexistent", updatedInfo);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUserAsync_WithValidId_DeletesUser()
        {
            // Arrange
            var user = new User
            {
                Id = "deleteuser",
                  FirstName = "Delete",
                 LastName = "Me",
                Email = "delete@example.com"
            };
            currContext.Users.Add(user);
            await currContext.SaveChangesAsync();

            // Act
            var result = await userService.DeleteUserAsync("deleteuser");


            // Assert
            Assert.True(result);

            var deletedUser = await currContext.Users.FindAsync("deleteuser");


            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task DeleteUserAsync_WithInvalidId_ReturnsFalse()
        {
            // Act
            var result = await userService.DeleteUserAsync("nonexistent");




            // Assert
            Assert.False(result);
        }
    }

}
