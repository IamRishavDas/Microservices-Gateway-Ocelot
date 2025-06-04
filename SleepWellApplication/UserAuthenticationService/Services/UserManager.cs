using UserAuthenticationService.Models;

namespace UserAuthenticationService.Services
{
    public class UserManager
    {
        private static List<User> users = new()
        {
            new(){UserName = "Rishav", Password = "rishav", Role = "Admin"},
            new(){UserName = "Rohit", Password = "rohit", Role = "User"},
        };

        public List<User> GetUsers()
        {
            return users;
        }

        public User CreateUser(User user)
        {
            users.Add(user);
            return user;
        }
    }
}
