using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        AccountController accountController;
        public UnitTest1()
        {
            accountController = new(userService, emailService);
        }

        [Fact]
        public void Test1()
        {
            var result = accountController.Token("adokuchaeva11@gmail.com", "aaaaaaaaaa").Result as JsonResult;
            Assert.Null(result.StatusCode);
        }


        [Fact]
        public void Test2()
        {
            var result = accountController.Registrate("adokuchaeva11@gmail.com", "aaaaaaaaaa").Result;
            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public void Test3()
        {
            var result = accountController.Token("adokuchaeva11@gmail.com", "aaa").Result as JsonResult;
            Assert.Null(result);
        }

        [Fact]
        public void Test4()
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
    }
}