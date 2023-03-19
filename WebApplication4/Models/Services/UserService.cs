using System.Collections.Generic;
using WebApplication4.Models.Interfaces;
using System.Linq;
using WebApplication4.Database;

namespace WebApplication4.Models.Services
{
    public class UserService:IUserService
    {
        private List<User> _users = new List<User>(){
            new User(){ Id = 0, Email = "a@mail.com", Password = "1111", Role = "user"},
            new User(){ Id = 1, Email = "b@mail.com", Password = "1111", Role = "admin"},
            new User(){ Id = 2, Email = "c@mail.com", Password = "1111", Role = "user"},
            new User(){ Id = 3, Email = "d@mail.com", Password = "1111", Role = "admin"}
        };

        /*IEnumerable<User> IUserService.GetAllUsers()
        {
            return _users;
        }

        User IUserService.GetUserById(int id)
        {
            return _users.Where(user => user.Id == id).FirstOrDefault();
        }

        IEnumerable<User> IUserService.GetUsersByRole(string role)
        {
            return _users.Where(user => user.Role == role);
        }*/

        User IUserService.GetUserByEmail(string email)
        {
           // return _users.Where(user => user.Email == email).FirstOrDefault()
           return DatabaseWRK.GetUserByEmailAsync(email);
        }
    }
}
