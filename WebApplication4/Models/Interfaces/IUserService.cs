using System.Collections.Generic;

namespace WebApplication4.Models.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> AddNewUser(User user);
        Task<bool> UpdateUserInfo(UserInfo user);
        Task<UserInfo> GetUserInfo(string email);
        Task<List<Specialty>> GetAllSpecialties();
        Task<bool> SaveUserSpecialties(List<Specialty> list);
        Task<List<Specialty>> GetUserSpecialties(string email);
        Task<bool> UpdateUserExams(Exams exams);
        Task<Exams> GetUserExams(string email);
    }
}
