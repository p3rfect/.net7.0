using System.Collections.Generic;

namespace WebApplication4.Models.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> AddNewUser(User user);
        Task<bool> UpdateUserInfo(UserInfo user, string email);
        Task<UserInfo> GetUserInfo(string email);
        Task<List<Specialty>> GetAllSpecialties();
        Task<bool> SaveUserSpecialties(List<Specialty> list, string email);
        Task<List<Specialty>> GetUserSpecialties(string email);
        Task<bool> UpdateUserExams(Exams exams, string email);
        Task<Exams> GetUserExams(string email);
    }
}
