using System.Collections.Generic;
using WebApplication4.Models.Interfaces;
using System.Linq;
using WebApplication4.Database;

namespace WebApplication4.Models.Services
{
    public class UserService:IUserService
    {
        public async Task<bool> AddNewUser(User user)
        {
            return await DatabaseWRK.AddNewUserAsync(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
           return await DatabaseWRK.GetUserByEmailAsync(email);
        }
    }
}
