using BookExchangePlatform.Models;

namespace BookExchangePlatform.Services.Interfaces
{
    public interface IMovieService
    {
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<Movie> UpdateMovieAsync(int id, Movie movie);
        Task<bool> DeleteMovieAsync(int id);

        Task<Movie?> GetMovieWithOwnerAsync(int id);


        //UserListPopulation
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetFirstUserAsync();
    }
}
