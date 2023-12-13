using UserDemoApp.Models;

namespace UserDemoApp.Repository
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(string id, User user, CancellationToken cancellationToken = default);
        Task SeedDataIfEmpty();
    }
}
