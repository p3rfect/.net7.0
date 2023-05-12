using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using WebApplication4.Controllers;
using WebApplication4.Models.Interfaces;
using WebApplication4.Models.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using WebApplication4.jwt;
using WebApplication4.Models;

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

        [Fact]
        public void Test2()
        {
            var result = accountController.Registrate("adokuchaeva11@gmail.com", "aaaaaaaaaa").Result;
            Assert.True(result is BadRequestObjectResult);
        }
    }
}