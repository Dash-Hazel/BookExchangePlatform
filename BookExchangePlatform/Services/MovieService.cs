using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BookExchangePlatform.Services
{
    public class MovieService:IMovieService
    {



        private readonly BookExchangeDbContext currContext;

        public MovieService(BookExchangeDbContext context)
        {
            currContext = context;
        }

        public async Task<List<Movie>> GetAllMoviesAsync(string? search = null, int page = 1, int pageSize = 10)
        {
            var query = currContext.Movies
                  .Include(b => b.Owner)
                  .AsQueryable();

            if (!string.IsNullOrEmpty(search))  //Searching
                query = query.Where(b => b.Title.Contains(search) || b.Director.Contains(search));

            //Pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await currContext.Movies
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Movie?> GetMovieWithOwnerAsync(int id)
        {
            return await currContext.Movies
                .Include(m => m.Owner)
                .Include(m => m.Reviews)
                .ThenInclude(r => r.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            if (movie.OwnerId == null)
            {
                var user = await currContext.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new ArgumentException("User cant be null");
                }

                
                movie.OwnerId = user.Id;
            }

            currContext.Movies.Add(movie);

            var saveResult = await currContext.SaveChangesAsync();
            return movie;

        }
        public async Task<Movie> UpdateMovieAsync(int id, Movie movie)
        {
            if (movie.OwnerId == null)
            {
                var originalMovie = await currContext.Movies
                    .Where(o => o.Id == id)
                    .FirstOrDefaultAsync();

                if (originalMovie != null)
                {
                    movie.OwnerId = originalMovie.OwnerId;
                }
            }

            var currMovie = await currContext.Movies
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();

            if (currMovie == null)
            {
                return null;
            }


            currMovie.Title = movie.Title;
            currMovie.Director = movie.Director;
            currMovie.Genre = movie.Genre;
            currMovie.ReleaseYear = movie.ReleaseYear;
            currMovie.Resume = movie.Resume;
            currMovie.OwnerId = movie.OwnerId;
            await currContext.SaveChangesAsync();
            return currMovie;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {

            var movie = await currContext.Movies
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return false;
            }

            currContext.Movies.Remove(movie);
            await currContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await currContext.Users.ToListAsync();

        }

        public async Task<User?> GetFirstUserAsync()
        {
            return await currContext.Users.FirstOrDefaultAsync();
        }
    }
}
