using BookExchangePlatform.Data;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace BookExchangePlatform.Services
{
    public class UserService : IUserService
    {
        private readonly BookExchangeDbContext currContext;

        public UserService(BookExchangeDbContext context)
        {
            currContext = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await currContext.Users
                .Include(u => u.Books)
                .Include(u => u.Movies)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            if (id == null) return null;

            var user = await currContext.Users
                .Include(u => u.Books)
                .Include(u => u.Movies)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null) return null;
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        { 
            currContext.Users.Add(user);
            var saveResult = await currContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(string id, User updatedUser)
        { 

            var existingUser = await currContext.Users
                .Include(u => u.Books)
                .Include(u => u.Movies)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (existingUser == null) return null;

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;

            var saveResult = await currContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await currContext.Users
                .Include(u => u.Books)
                .Include(u => u.Movies)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null) return false;

            currContext.Users.Remove(user);
            var saveResult = await currContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserWithDetailsAsync(string id)
        {
            if (id == null) return null;
            var user = await currContext.Users
                .Include(u=> u.Books)
                .Include(u=> u.Movies)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            if (user == null) return null;
            return user;
        }
    }
}
