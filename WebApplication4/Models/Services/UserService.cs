using System.Collections.Generic;
using WebApplication4.Models.Interfaces;
using System.Linq;
using WebApplication4.Database;
//add method

namespace WebApplication4.Models.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> AddNewUser(User user)
        {
            return await DatabaseWRK.AddNewUserAsync(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await DatabaseWRK.GetUserByEmailAsync(email);
        }

        public async Task<bool> UpdateUserInfo(UserInfo user, string email)
        {
            //add method
            await Task.Delay(0);
            return true;
        }

        public async Task<UserInfo> GetUserInfo(string email)
        {
            //add method
            await Task.Delay(0);
            return new UserInfo();
        }

        public async Task<List<Specialty>> GetAllSpecialties()
        {
            return await DatabaseWRK.GetAllSpecialtiesAsync();
        }

        public async Task<bool> UpdateUserSpecialties(UserSpecialties specialties, string email)
        {
            //add method
            await Task.Delay(0);
            return true;
        }

        public async Task<UserSpecialties> GetUserSpecialties(string email)
        {
            //return await DatabaseWRK.GetUserSpecialtiesAsync(email);
            await Task.Delay(0);
            return new UserSpecialties();
        }

        public async Task<bool> UpdateUserExams(Exams exams, string email)
        {
            return await DatabaseWRK.UpdateUserExamsAsync(exams, email);
        }

        public async Task<Exams> GetUserExams(string email)
        {
            return await DatabaseWRK.GetUserExamsAsync(email);
        }

        public async Task<bool> ConfirmEmail(string email)
        {

            //add method
            await Task.Delay(0);
            return true;
        }
    }
}
