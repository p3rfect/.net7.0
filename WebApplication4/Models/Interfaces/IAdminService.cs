namespace WebApplication4.Models.Interfaces
{
    public interface IAdminService
    {
        Task<List<string>> GetAllUsersEmails();

        Task<Tuple<UserInfo, Exams, UserSpecialties>> GetUser(string email);

        Task<List<bool>> UpdateUser(string email, UserInfo info, Exams exams, UserSpecialties specialties);

        Task<bool> DeleteUser(string email);

        Task<bool> ConfirmUser(string email);

        Task<bool> AcceptUser(string email);

        Task<bool> Enroll();
    }
}
