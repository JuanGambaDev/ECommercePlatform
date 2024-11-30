using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Models;
using UserService.Repositories;
using UserService.Data;
using Xunit;

namespace UserService.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private async Task<UserServiceContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<UserServiceContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceTestDb")
                .EnableSensitiveDataLogging()
                .Options;

            var context = new UserServiceContext(options);

            // Agregar datos iniciales
            if (!context.Users.Any())
            {
                context.Users.AddRange(new List<User>
                {
                    new User { UserId = 1, Name = "John", Email = "john@example.com", PasswordHash= "d09b14dd0dddb9e892d9150fd1200f45e89b1911f699a34faa3cf90b0dd2c3e9" },
                    new User { UserId = 2, Name = "Jane", Email = "jane@example.com", PasswordHash= "d09b14dd0dddb9e892d9150fd1200f45e89b1911f699a34faa3cf90b0dd2c3e9" }
                });

                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new UserRepository(context);

            // Act
            var users = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new UserRepository(context);
            var userId = 1;

            // Act
            var user = await repository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal("John", user.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new UserRepository(context);
            var userId = 99;

            // Act
            var user = await repository.GetByIdAsync(userId);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task CreateAsync_AddsUserToDatabase()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new UserRepository(context);
            var newUser = new User
            {
                UserId = 3,
                Name = "Alice",
                Email = "alice@example.com",
                PasswordHash= "d09b14dd0dddb9e892d9150fd1200f45e89b1911f699a34faa3cf90b0dd2c3e9" 
            };

            // Act
            var result = await repository.CreateAsync(newUser);

            // Assert
            Assert.True(result);
            Assert.Equal(3, context.Users.Count());
            var user = await context.Users.FindAsync(3);
            Assert.NotNull(user);
            Assert.Equal("Alice", user.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUserFromDatabase_WhenUserExists()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new UserRepository(context);
            var userId = 1;

            // Act
            var result = await repository.DeleteAsync(userId);

            // Assert
            Assert.True(result);
            //Assert.Equal(1, context.Users.Count());
            var user = await context.Users.FindAsync(userId);
            Assert.Null(user);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new UserRepository(context);
            var userId = 99;

            // Act
            var result = await repository.DeleteAsync(userId);

            // Assert
            Assert.False(result);
            //Assert.Equal(, context.Users.Count());
        }


    }
}
