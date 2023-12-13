using UserDemoApp.Models;
using UserDemoApp.Repository;

namespace UserDemoApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userRepository.CreateAsync(user, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(string id, User user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepository.UpdateAsync(id, user, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepository.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
