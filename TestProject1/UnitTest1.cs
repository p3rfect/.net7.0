using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using WebApplication4.Controllers;
using WebApplication4.Models;
using WebApplication4.Models.Interfaces;
using WebApplication4.Models.Services;

namespace TestProject1
{
    public class UnitTest1
    {
        IUserService userService = new UserService();
        IEmailService emailService = new EmailService();
        IAdminService adminService = new AdminService();

        AccountController accountController;
        AdminController adminController;
        public UnitTest1()
        {
            accountController = new(userService, emailService);
            adminController = new(adminService);
        }

        [Fact]
        public void TokenValidParams()
        {
            var result = accountController.Token("adokuchaeva11@gmail.com", "aaaaaaaaaa").Result as JsonResult;
            Assert.Null(result.StatusCode);
        }

        [Fact]
        public void TokenInvalidParams()
        {
            var result = accountController.Token("adokuchaeva11@gmail.com", "gjhbk").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void RegistrateExistedUser()
        {
            var result = accountController.Registrate("adokuchaeva11@gmail.com", "aaaaaaaaaa").Result;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void AllExamsList()
        {
            var result = accountController.GetAllSpecialties().Result as JsonResult;
            List<Specialty> res1 = new()
            {
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������ ������ � ����� ����������� ����������� �������������� ������", IsPhysics=true, SpecialtyCode = "31 03 04 06" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� �������������� � ������������ ����������-����������� ����������� �������", IsPhysics=true, SpecialtyCode = "39 02 02" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ������������� � ������������ �������������� ���������������� �������", IsPhysics=true, SpecialtyCode = "39 02 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ����������-����������� ����������-���������� �������", IsPhysics=true, SpecialtyCode = "36 04 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ����������� �����������", IsPhysics=true, SpecialtyCode = "39 02 03" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ����������� ������� ������������", IsPhysics=true, SpecialtyCode = "39 02 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ��������������� ��������� �������", IsPhysics=true, SpecialtyCode ="39 03 02" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ���������-��������������� ����������� �������������� ����������", IsPhysics=true, SpecialtyCode ="58 01 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� �������������� ������� � ���������� (� ����������� ������������ ������������)", IsPhysics=true, SpecialtyCode = "40 05 01-09" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� �������������� ������� � ���������� (� ������-�����������)", IsPhysics=true, SpecialtyCode = "40 05 01-10" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� �������������� � ������������ ����������-����������� ����������� �������", IsPhysics=true, SpecialtyCode = "39 02 02" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ����������� ������� ������������", IsPhysics=true, SpecialtyCode = "39 03 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������� �������������� ��������������� ��������� �������", IsPhysics=true, SpecialtyCode = "39 03 02" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� �������������� ���������� � ���������� ������������������ ������� ��������� ����������", IsPhysics=true, SpecialtyCode = "53 01 02" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� �������������� ���������� � ���������� ������������� ���������", IsPhysics=true, SpecialtyCode = "40 03 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� �������������� ���������� � ���������� �������������� ������� � ���������� (� ������� ���������)", IsPhysics=true, SpecialtyCode ="40 05 01-12" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� �������������� ���������� � ���������� ������������ �����������", IsPhysics=true, SpecialtyCode ="36 04 02"},
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� �������������� ���������� � ���������� �������������� ���������� � ���������� � ����������� ��������", IsPhysics=true, SpecialtyCode ="53 01 07" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������ ������ � ����� ����������� � ���������� ����������������", IsPhysics=true, SpecialtyCode ="40 40 01" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������ ������ � ����� ����������� �������������� ��������", IsPhysics=true, SpecialtyCode ="40 02 02" },
                new(){FinancingFormPeriod = new(), SpecialtyFacultyAndName="��������� ������������ ������ � ����� �������������� ������, ������� � ����", IsPhysics=true, SpecialtyCode = "40 02 01" }
            };

            var res = result.Value as List<Specialty>;
            for (int i = 0; i < res.Count; i++)
            {
                Assert.Equal(res[i].SpecialtyFacultyAndName, res1[i].SpecialtyFacultyAndName);
                Assert.Equal(res[i].SpecialtyCode, res1[i].SpecialtyCode);
                Assert.Equal(res[i].FinancingFormPeriod, res1[i].FinancingFormPeriod);
                Assert.Equal(res[i].IsPhysics, res1[i].IsPhysics);
            }
        }

        [Fact]
        public void UpdateValidUserInfo()
        {
            var result = accountController.UpdateUserInfo(JsonSerializer.Serialize(new UserInfo()),"adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.True(result is OkObjectResult);
        }

        [Fact]
        public void UpdateInvalidUserInfo()
        {
            var result = accountController.UpdateUserInfo(JsonSerializer.Serialize(new UserInfo()), "adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void GetValidUserInfo()
        {
            var result = accountController.GetUserInfo("adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.Equal(result.Value, new UserInfo());
        }

        [Fact]
        public void GetInvalidUserInfo()
        {
            var result = accountController.GetUserInfo("adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void UpdateValidUserSpecialties()
        {
            var result = accountController.UpdateUserSpecialties(JsonSerializer.Serialize(new UserSpecialties()), "adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.True(result is OkObjectResult);
        }

        [Fact]
        public void UpdateInvalidUserSpecialties()
        {
            var result = accountController.UpdateUserSpecialties(JsonSerializer.Serialize(new UserSpecialties()), "adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void GetValidUserSpecialties()
        {
            var result = accountController.GetUserSpecialties("adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.Equal(result.Value, new UserSpecialties());
        }

        [Fact]
        public void GetInvalidUserSpecialties()
        {
            var result = accountController.GetUserSpecialties("adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void UpdateValidUserExams()
        {
            var result = accountController.UpdateUserExams(JsonSerializer.Serialize(new Exams()), "adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.True(result is OkObjectResult);
        }

        [Fact]
        public void UpdateInvalidUserExams()
        {
            var result = accountController.UpdateUserExams(JsonSerializer.Serialize(new Exams()), "adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void GetValidUserExams()
        {
            var result = accountController.GetUserExams("adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.Equal(result.Value, new Exams());
        }

        [Fact]
        public void GetInvalidUserExams()
        {
            var result = accountController.GetUserExams("adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void AllUsersList()
        {
            var result = adminController.GetAllUsers().Result as JsonResult;
            List<string> emails = new(){};

            var res = result.Value as List<string>;
            for (int i = 0; i < res.Count; i++)
            {
                Assert.Equal(emails[i], res[i]);
            }
        }



        [Fact]
        public void GetValidUser()
        {
            var result = adminController.GetUser("adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.Equal(result.Value, (new UserInfo(), new UserSpecialties(), new Exams()));
        }

        [Fact]
        public void GetInvalidUser()
        {
            var result = adminController.GetUser("adokua11@gmail.com").Result as JsonResult;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void UpdateValidUser()
        {
            var result = adminController.UpdateUser("adokuchaeva11@gmail.com", JsonSerializer.Serialize(new UserInfo()), JsonSerializer.Serialize(new Exams()), JsonSerializer.Serialize(new UserSpecialties())).Result as JsonResult;
            Assert.Equal(result.Value, (true, true, true));
        }

        [Fact]
        public void UpdateInvalidUser()
        {
            var result = adminController.UpdateUser("adokua11@gmail.com", JsonSerializer.Serialize(new UserInfo()), JsonSerializer.Serialize(new Exams()), JsonSerializer.Serialize(new UserSpecialties())).Result as JsonResult;
            Assert.NotEqual(result.Value, (true, true, true));
        }

        [Fact]
        public void ConfirmValidEmail()
        {
            var result = adminController.ConfirmUser("adokuchaeva11@gmail.com").Result as JsonResult;
            Assert.Equal(result.Value, true);
        }

        [Fact]
        public void ConfirmInvalidEmail()
        {
            var result = adminController.ConfirmUser("adokua11@gmail.com").Result as JsonResult;
            Assert.Equal(result.Value, false);
        }
    }
}