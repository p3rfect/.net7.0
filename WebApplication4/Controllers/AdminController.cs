using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication4.Models;
using WebApplication4.Models.Interfaces;
using WebApplication4.Models.Services;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/admin/allusers/get")]
        async public Task<IActionResult> GetAllUsers()
        {
            List<string> result = await _adminService.GetAllUsersEmails();
            return Json(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/admin/user/get")]
        async public Task<IActionResult> GetUser(string email)
        {
            var result = await _adminService.GetUser(email);
            return Json(result);
        }

        [Authorize(Roles="admin")]
        [HttpPost("/admin/user/update")]
        async public Task<IActionResult> UpdateUser(string email, string userInfoJsonStr, string userExamsJsonStr, string userSpecialtiesJsonStr)
        {
            UserInfo info = JsonSerializer.Deserialize<UserInfo>(userInfoJsonStr);
            Exams exams = JsonSerializer.Deserialize<Exams>(userExamsJsonStr);
            UserSpecialties specialties = JsonSerializer.Deserialize<UserSpecialties>(userSpecialtiesJsonStr);
            var result =  await _adminService.UpdateUser(email, info, exams, specialties); 
            return Json(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("/admin/user/delete")]
        async public Task<IActionResult> DeleteUser(string email)
        {
            bool result = await _adminService.DeleteUser(email);
            return Json(result);
        }
    }
}
