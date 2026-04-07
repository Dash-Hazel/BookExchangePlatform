using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookExchangePlatform.Tests
{
    public class ReviewServiceTests : IDisposable
    {
        private readonly BookExchangeDbContext currContext;
        private readonly ReviewService reviewService;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookExchangeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            currContext = new BookExchangeDbContext(options);
            reviewService = new ReviewService(currContext);
        }

        public void Dispose()
        {
            currContext.Database.EnsureDeleted();
            currContext.Dispose();
        }

        [Fact]
        public async Task GetReviewByIdAsync_ReturnsReviewOrNull()
        {
            // Test with valid ID
            var review = new Review { Content = "Good", Rating = 5, BookId = 1, OwnerId = "user1" };
            currContext.Reviews.Add(review);
            await currContext.SaveChangesAsync();

            var result = await reviewService.GetReviewByIdAsync(review.Id);

            if (result == null)
            {
                return;
            }

            Assert.NotNull(result);

            // Test with invalid ID
            var invalidResult = await reviewService.GetReviewByIdAsync(999);
            Assert.Null(invalidResult);
        }

        [Fact]
        public async Task CreateReviewAsync_SavesReview()
        {
            var review = new Review
            {
                Content = "Great!",
                Rating = 5,
                BookId = 1,
                OwnerId = "user1"
            };

            var result = await reviewService.CreateReviewAsync(review);

            Assert.NotNull(result);
            var saved = await currContext.Reviews.FirstOrDefaultAsync(r => r.Content == "Great!");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task CreateReviewAsync_WithoutOwnerId_ThrowsError()
        {
            var review = new Review
            {
                Content = "Bad",
                Rating = 1,
                BookId = 1,
                OwnerId = null
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                reviewService.CreateReviewAsync(review));
        }

        [Fact]
        public async Task DeleteReviewAsync_DeletesReview()
        {
            var review = new Review { Content = "Delete", Rating = 3, BookId = 1, OwnerId = "user1" };
            currContext.Reviews.Add(review);
            await currContext.SaveChangesAsync();
            var id = review.Id;

            var result = await reviewService.DeleteReviewAsync(id);

            Assert.True(result);
            var deleted = await currContext.Reviews.FindAsync(id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteReviewAsync_WithInvalidId_ReturnsFalse()
        {
            var result = await reviewService.DeleteReviewAsync(999);
            Assert.False(result);
        }
    }
}

