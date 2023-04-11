using System.Collections.Generic;

namespace WebApplication4.Models.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
        Task<bool> AddNewUser(User user);
        Task<bool> UpdateUserInfo(UserInfo user);
        Task<UserInfo> GetUserInfo(string email);
        Task<List<Specialties>> GetAllSpecialties();
        Task<bool> SaveUserSpecialties(List<Specialties> list);
        Task<List<Specialties>> GetUserSpecialties(string email);
        Task<bool> UpdateUserExams(Exams exams);
        Task<Exams> GetUserExams(string email);
    }
}
