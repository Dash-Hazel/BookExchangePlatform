using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookExchangePlatform.Tests
{
    public class MovieServiceTests: IDisposable
    {
        private readonly BookExchangeDbContext currContext;
        private readonly MovieService movieService;

        public MovieServiceTests()
        {

            var options = new DbContextOptionsBuilder<BookExchangeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            currContext = new BookExchangeDbContext(options);
            movieService = new MovieService(currContext);
        }

        public void Dispose()
        {
            currContext.Database.EnsureDeleted();
            currContext.Dispose();
        }

        [Fact]
        public async Task GetMoviekByIdAsync_WhenBookExists_ShouldReturnThatBook()
        {
            //Arrange
            var movie = new Movie { Title = "Surrounded by idiots",  Resume = "Resume of a Movie", Director = "Thomas Erickson", Genre = "Comedy", OwnerId = "1" };
            currContext.Movies.Add(movie);
            await currContext.SaveChangesAsync();

            var sabedMovieId = movie.Id;
            //Act
            var result = await movieService.GetMovieByIdAsync(sabedMovieId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(sabedMovieId, result.Id);
            Assert.Equal("Surrounded by idiots", result.Title);
        }

        [Fact]
        public async Task GetMovieByIdAsync_WithInvalidId_ReturnsNull()
        {
            // ACT
            var result = await movieService.GetMovieByIdAsync(999);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateMovieAsync_WithValidBook_SavesToDatabase()
        {
            //Arrange

            var movie = new Movie
            {
                Title = "New Book",
                Director = "Some Author",
                OwnerId = "1",
                Resume = "Resume of the movie",
                Genre = "Horror"

            };

            // ACT
            var result = await movieService.CreateMovieAsync(movie);

            // ASSERT
            Assert.NotNull(result);
            var savedMovie = await currContext.Movies.FirstOrDefaultAsync(b => b.Title == "New Book");
            Assert.NotNull(savedMovie);
        }

        [Fact]
        public async Task UpdateMovieAsync_WithValidId_UpdatesBook()
        {
            //Arrange

            var movie = new Movie
            {
                Title = "New Book",
                Director = "Some Author",
                OwnerId = "1",
                Resume = "Resume of the movie",
                Genre = "Horror"

            };
            currContext.Movies.Add(movie);
            await currContext.SaveChangesAsync();

            var updatedMovie = new Movie
            {
                Title = "New Title",
                Director = "Author",
                OwnerId = "1",
                Resume = "Resume of the movie",
                Genre = "Horror"
            };


            // ACT
            var result = await movieService.UpdateMovieAsync(movie.Id, updatedMovie);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("New Title", result.Title);
        }

        [Fact]
        public async Task UpdateMovieAsync_WithInvalidId_ReturnsNull()
        {
            // ARRANGE
            var updatedMovie= new Movie
            {
                Title = "New Book",
                Director = "Some Author",
                OwnerId = "1",
                Resume = "Resume of the movie",
                Genre = "Horror"

            };

            // ACT
            var result = await movieService.UpdateMovieAsync(999, updatedMovie);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteMovieAsync_WithValidId_DeletesMovie()
        {
            // ARRANGE
            var movie = new Movie
            {
                Title = "New Book",
                Director = "Some Author",
                OwnerId = "1",
                Resume = "Resume of the movie",
                Genre = "Horror"

            };

            currContext.Movies.Add(movie);
            await currContext.SaveChangesAsync();
            var movieId = movie.Id;


            // ACT
            var result = await movieService.DeleteMovieAsync(movieId);

            // ASSERT
            Assert.True(result);
            var deletedMovie = await currContext.Movies.FindAsync(movieId);
            Assert.Null(deletedMovie);
        }


        [Fact]
        public async Task DeleteMovieAsync_WithInvalidId_ReturnsFalse()
        {
            // ACT
            var result = await movieService.DeleteMovieAsync(999);

            // ASSERT
            Assert.False(result);
        }
    }
}
