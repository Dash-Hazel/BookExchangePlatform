using BookExchangePlatform.Models;

namespace BookExchangePlatform.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(string id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(string id, User user);
        Task<bool> DeleteUserAsync(string id);

        Task<User> GetUserWithDetailsAsync(string id);
    }
}
