using Moq;
using UserDemoApp.Models;
using UserDemoApp.Repository;
using UserDemoApp.Services;

namespace UserDemoApp.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfUsers()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var users = new List<User>
            {
                new User { Id = "1", FirstName = "John", LastName = "Doe", Email="john.doe@test.com" },
                new User { Id = "2", FirstName = "Jane", LastName = "Smith", Email="jane.smith@test.com" }
            };

            var cancellationToken = new CancellationToken();

            userRepositoryMock.Setup(repo => repo.GetAllAsync(cancellationToken))
                              .ReturnsAsync(users);

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = await userService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserById()
        {
            // Arrange
            var userId = "1";
            var userRepositoryMock = new Mock<IUserRepository>();
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            var cancellationToken = new CancellationToken();

            userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, cancellationToken))
                              .ReturnsAsync(user);

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = await userService.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateUser()
        {
            // Arrange
            var newUser = new User { FirstName = "New", LastName = "User" };
            var userRepositoryMock = new Mock<IUserRepository>();
            var cancellationToken = new CancellationToken();

            userRepositoryMock.Setup(repo => repo.CreateAsync(newUser, cancellationToken))
                              .ReturnsAsync(newUser);

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = await userService.CreateAsync(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.FirstName, result.FirstName);
            Assert.Equal(newUser.LastName, result.LastName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WithCancellationToken()
        {
            // Arrange
            var userId = "1";
            var updatedUser = new User { Id = userId, FirstName = "UpdatedName", LastName = "UpdatedLast" };
            var cancellationToken = new CancellationToken();
            var userRepositoryMock = new Mock<IUserRepository>();

            // Set up the repository to throw an exception of type InvalidOperationException
            userRepositoryMock.Setup(repo => repo.UpdateAsync(userId, updatedUser, cancellationToken))
                              .ThrowsAsync(new InvalidOperationException("Invalid user update"));

            var userService = new UserService(userRepositoryMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateAsync(userId, updatedUser, cancellationToken));
            Assert.Equal("Invalid user update", ex.Message);
        }


        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WithCancellationToken()
        {
            // Arrange
            var userId = "1";
            var cancellationToken = new CancellationToken();
            var userRepositoryMock = new Mock<IUserRepository>();

            // Set up the repository to throw an exception of type InvalidOperationException
            userRepositoryMock.Setup(repo => repo.DeleteAsync(userId, cancellationToken))
                              .ThrowsAsync(new InvalidOperationException("Invalid user ID"));

            var userService = new UserService(userRepositoryMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.DeleteAsync(userId, cancellationToken));
            Assert.Equal("Invalid user ID", ex.Message);
        }

    }
}