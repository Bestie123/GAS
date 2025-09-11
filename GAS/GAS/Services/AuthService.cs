using System.Linq;
using GAS.Data;
using GAS.Models;

namespace GAS.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService()
        {
            _db = new AppDbContext();
        }

        public bool Register(string fullName, string login, string password, out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                error = "Заполните все поля";
                return false;
            }

            if (_db.Users.Any(u => u.Username == login))
            {
                error = "Логин уже занят";
                return false;
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            _db.Users.Add(new User
            {
                FullName = fullName,
                Username = login,
                PasswordHash = hash
            });
            _db.SaveChanges();
            return true;
        }

        public User? Login(string login, string password)
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == login);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return user;
            return null;
        }
    }
}

