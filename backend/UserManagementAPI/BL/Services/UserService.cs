using UserManagementAPI.DAL.Context;
using UserManagementAPI.DAL.Dtos.Results;
using UserManagementAPI.DAL.Models;

namespace UserManagementAPI.BL.Services
{
    public interface IUserService
    {
        Result<User> ValidateUser(string username, string password);
        Result<User> SignUp(string username, string password);
        Result<User> UpdateUserLastLogIn(string username);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _passwordService;

        public UserService(ApplicationDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public Result<User> ValidateUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }

            if (!_passwordService.Verify(password, user.PasswordHash))
            {
                return Result<User>.Failure("Invalid password");
            }

            return Result<User>.Success(user);
        }

        public Result<User> SignUp(string username, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                return Result<User>.Failure("User already exists");
            }

            var passwordHash = _passwordService.Hash(password);

            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                LastLogIn = null,
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Result<User>.Success(newUser);
        }

        public Result<User> UpdateUserLastLogIn(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }

            user.LastLogIn = DateTime.UtcNow;
            _context.SaveChanges();

            return Result<User>.Success(user);  // Returning the updated user
        }
    }
}
