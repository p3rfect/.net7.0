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

        [Authorize(Roles = "admin, trustee")]
        [HttpGet("/admin/allusers/get")]
        async public Task<IActionResult> GetAllUsers()
        {
            List<string> result = await _adminService.GetAllUsersEmails();
            return Json(result);
        }

        [Authorize(Roles = "admin, trustee")]
        [HttpGet("/admin/user/get")]
        async public Task<IActionResult> GetUser(string email)
        {
            var result = await _adminService.GetUser(email);
            return Json(result);
        }

        [Authorize(Roles = "admin, trustee")]
        [HttpPost("/admin/user/update")]
        async public Task<IActionResult> UpdateUser(string email, string userinfo, string userexams, string userspecialties)
        {
            UserInfo info = JsonSerializer.Deserialize<UserInfo>(userinfo);
            UserSpecialties specialties = JsonSerializer.Deserialize<UserSpecialties>(userspecialties);
            Exams exams = JsonSerializer.Deserialize<Exams>(userexams);
            var result = await _adminService.UpdateUser(email, info, exams, specialties);
            return Json(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("/admin/user/delete")]
        async public Task<IActionResult> DeleteUser(string email)
        {
            bool result = await _adminService.DeleteUser(email);
            return Json(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("/admin/user/confirm")]
        async public Task<IActionResult> ConfirmUser(string email)
        {
            bool result = await _adminService.ConfirmUser(email);
            return Json(result);
        }

        [Authorize(Roles = "trustee")]
        [HttpPost("/admin/user/accept")]
        async public Task<IActionResult> AcceptUser(string email)
        {
            bool result = await _adminService.AcceptUser(email);
            return Json(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("/admin/enroll")]
        async public Task<IActionResult> Enroll()
        {
            bool result = await _adminService.Enroll();
            return Json(result);
        }
    }
}
