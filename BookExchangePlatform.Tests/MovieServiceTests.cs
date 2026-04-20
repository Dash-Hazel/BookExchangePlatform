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


        [Fact]
        public async Task GetAllMoviesAsync_ReturnsAllMovies()
        {
            // ARRANGE
            var user = new User { Id = "1", UserName = "user@test.com", Email = "user@test.com", FirstName = "Test", LastName = "User" };

            currContext.Users.Add(user);

            await currContext.SaveChangesAsync();

            currContext.Movies.AddRange(
                new Movie { Title = "Movie 1", Director = "Director 1", Resume = "Resume", Genre = "Action", OwnerId = "1" },


                new Movie { Title = "Movie 2", Director = "Director 2", Resume = "Resume", Genre = "Drama", OwnerId = "1" }
            );
            await currContext.SaveChangesAsync();


            // ACT

            var result = await movieService.GetAllMoviesAsync();


            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async Task GetAllMoviesAsync_WithSearch_ReturnsFilteredMovies()
        {
            // ARRANGE
            var user = new User { Id = "1", UserName = "user@test.com", Email = "user@test.com", FirstName = "Test", LastName = "User" };

            currContext.Users.Add(user);

            await currContext.SaveChangesAsync();

            currContext.Movies.AddRange(
                new Movie { Title = "Inception", Director = "Nolan", Resume = "Resume", Genre = "Sci-Fi", OwnerId = "1" },

                new Movie { Title = "The Godfather", Director = "Coppola", Resume = "Resume", Genre = "Crime", OwnerId = "1" }

            );
            await currContext.SaveChangesAsync();



            // ACT

            var result = await movieService.GetAllMoviesAsync(search: "Inception");


            // ASSERT
            Assert.Single(result);

            Assert.Equal("Inception", result[0].Title);
        }

        [Fact]
        public async Task GetAllMoviesAsync_WithPagination_ReturnsCorrectPage()
        {
            // ARRANGE

            var user = new User { Id = "1", UserName = "user@test.com", Email = "user@test.com", FirstName = "Test", LastName = "User" };
            currContext.Users.Add(user);

            await currContext.SaveChangesAsync();




            for (int i = 1; i <= 15; i++)
                currContext.Movies.Add(new Movie { Title = $"Movie {i}", Director = "Director", Resume = "Resume", Genre = "Genre", OwnerId = "1" });

            await currContext.SaveChangesAsync();


            // ACT
            var result = await movieService.GetAllMoviesAsync(page: 2, pageSize: 10);

            // ASSERT
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task GetMovieWithOwnerAsync_WhenMovieExists_ReturnsMovieWithOwner()
        {
            // ARRANGE
            var user = new User { Id = "user1", UserName = "test@test.com", Email = "test@test.com", FirstName = "Test", LastName = "User" };

            currContext.Users.Add(user);


            var movie = new Movie { Title = "Test Movie", Director = "Director", Resume = "Resume", Genre = "Action", OwnerId = "user1" };


            currContext.Movies.Add(movie);

            await currContext.SaveChangesAsync();

            // ACT
            var result = await movieService.GetMovieWithOwnerAsync(movie.Id);


            // ASSERT

            Assert.NotNull(result);
            Assert.NotNull(result.Owner);

        }

        [Fact]
        public async Task GetMovieWithOwnerAsync_WithInvalidId_ReturnsNull()
        {
            // ACT
            var result = await movieService.GetMovieWithOwnerAsync(999);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // ARRANGE
            currContext.Users.AddRange(
                new User { Id = "1", UserName = "user1@test.com", Email = "user1@test.com", FirstName = "User", LastName = "One" },



                new User { Id = "2", UserName = "user2@test.com", Email = "user2@test.com", FirstName = "User", LastName = "Two" }
            );

            await currContext.SaveChangesAsync();

            // ACT
            var result = await movieService.GetAllUsersAsync();

            // ASSERT

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetFirstUserAsync_WhenUsersExist_ReturnsUser()
        {
            // ARRANGE
            currContext.Users.Add(new User { Id = "1", UserName = "user@test.com", Email = "user@test.com", FirstName = "User", LastName = "One" });


            await currContext.SaveChangesAsync();

            // ACT
            var result = await movieService.GetFirstUserAsync();


            // ASSERT
            Assert.NotNull(result);
        }
    }
}
