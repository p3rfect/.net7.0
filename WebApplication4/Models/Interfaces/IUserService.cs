using System.Collections.Generic;

namespace WebApplication4.Models.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> AddNewUser(User user);
    }
}
