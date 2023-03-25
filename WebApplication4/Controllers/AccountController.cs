﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication4.jwt;
using WebApplication4.Models;
using WebApplication4.Models.Interfaces;

namespace WebApplication4.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("/token")]
        async public Task<IActionResult> Token(string email, string password)
        {
            var identity = await GetIdentity(email, password);
            if (identity == null) 
                return BadRequest(new {errorText="Invalid user name or password"});
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
               issuer: AuthOptions.ISSUER, 
               audience: AuthOptions.AUDIENCE, 
               notBefore: now, 
               claims: identity.Claims, 
               expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)), 
               signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responce = new
            {
                access_token = encodedJwt,
                email = identity.Name
            };

            return Json(responce);
        }

        async private Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var connUser = await _userService.GetUserByEmail(email);
            if (connUser.Email != null && connUser.Password == password)
            {
                var Claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, connUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, connUser.Role)
                };
                ClaimsIdentity identity = new (Claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return identity;
            }
            return null;
        }

        [HttpPost("/register")]
        async public Task<IActionResult> Registrate(string email, string password)
        {
            User user = new();
            user.Email = email;
            user.Password = password;
            user.Role = "user";
            bool result = await _userService.AddNewUser(user);
            if(!result) return BadRequest(new { errorText = "User is already exist" });
            await SendMail(email, "submit letter", "");
            return Ok(result);
        }

        async private Task SendMail(string email, string subject, string message)
        { 
            await _emailService.SendEmail(email, subject, message);
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
