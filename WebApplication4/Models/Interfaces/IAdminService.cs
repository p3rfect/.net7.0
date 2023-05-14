namespace WebApplication4.Models.Interfaces
{
    public interface IAdminService
    {
        Task<List<string>> GetAllUsersEmails();

        Task<(UserInfo, Exams, UserSpecialties)> GetUser(string email);

        Task<(bool, bool, bool)> UpdateUser(string email, UserInfo info, Exams exams, UserSpecialties specialties);

        Task<bool> DeleteUser(string email);

        Task<bool> ConfirmUser(string email);
    }
}
