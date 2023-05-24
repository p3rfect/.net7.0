using WebApplication4.Database;
using WebApplication4.Models.Interfaces;

namespace WebApplication4.Models.Services
{
    public class AdminService : IAdminService
    {
        private IUserService _userService = new UserService();

        public async Task<bool> AcceptUser(string email)
        {
            return await DatabaseWRK.AcceptUserAsync(email);
        }

        public async Task<bool> ConfirmUser(string email)
        {
            return await _userService.ConfirmEmail(email);
        }

        public async Task<bool> DeleteUser(string email)
        {
            return await DatabaseWRK.DeleteUserAsync(email);
        }

        public async Task<List<string>> GetAllUsersEmails()
        {
            return await DatabaseWRK.GetAllUsersEmailsAsync();
        }

        public async Task<(UserInfo, Exams, UserSpecialties)> GetUser(string email)
        {
            return (await _userService.GetUserInfo(email), await _userService.GetUserExams(email), await _userService.GetUserSpecialties(email));
        }

        public async Task<(bool, bool, bool)> UpdateUser(string email, UserInfo info, Exams exams, UserSpecialties specialties)
        {
            return (await _userService.UpdateUserInfo(info, email), await _userService.UpdateUserExams(exams, email), await _userService.UpdateUserSpecialties(specialties, email));
        }

        public async Task<bool> Enroll()
        {
            return await Enrollment.EnrollmentUsers();
        }
    }
}
