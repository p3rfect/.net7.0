using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using WebApplication4.Controllers;
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
            accountController = new(userService, emailService );
        }

        [Fact]
        public void Test1()
        {
            
            var result = accountController.Token("adokuchaeva11@gmail.com", "aaaaaaaaaa").Result as JsonResult;
            Assert.Null(result.StatusCode);
        }
    }
}