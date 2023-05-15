using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using WebApplication4.jwt;
using WebApplication4.Models;
using WebApplication4.Models.Interfaces;
using WebApplication4.Models.Services;

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

        [HttpPost("/token/getnew")]
        async public Task<IActionResult> Token(string email, string password)
        {
            var identity = await GetIdentity(email, password);
            if (identity == null)
                return BadRequest(new { errorText = "Invalid user name or password" });
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
               issuer: AuthOptions.ISSUER,
               audience: AuthOptions.AUDIENCE,
               notBefore: now,
               claims: identity.Claims,
               expires: now.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
               signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responce = new
            {
                access_token = encodedJwt,
                email = identity.Name,
                role = identity.Claims.ToList()[1].Value
            };

            return Json(responce);
        }

        async private Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var connUser = await _userService.GetUserByEmail(email);

            if (connUser.Email != null && connUser.Password == password /*&& connUser.Confirmed==true*/)
            {
                var Claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, connUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, connUser.Role)
                };
                ClaimsIdentity identity = new(Claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return identity;
            }
            return null;
        }

        [Authorize]
        [HttpGet("/token/isvalid")]
        public IActionResult IsTokenValid()
        {
            return Json(true);
        }

        [HttpPost("/register")]
        async public Task<IActionResult> Registrate(string email, string password)
        {
            User user = new()
            {
                Email = email,
                Password = password,
                Role = "user",
                Confirmed = false
            };
            bool result = await _userService.AddNewUser(user);
            if (!result) return BadRequest(new { errorText = "User is already exist" });

            var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        user.Email,
                        protocol: HttpContext.Request.Scheme);

            //await SendMail(email, "Confirm your account", $"Confirm your registration by following the link: <a href='{callbackUrl}'>link</a>");

            return Ok(result);
        }

        async private Task SendMail(string email, string subject, string message)
        {
            await _emailService.SendEmail(email, subject, message);
        }

        [AllowAnonymous]
        public async Task<bool> ConfirmEmail(string email) 
        {
            return await _userService.ConfirmEmail(email);
        }

        [Authorize]
        [HttpPost("/user/info/update")]
        async public Task<IActionResult> UpdateUserInfo(UserInfo info, string email)
        {
            bool result = await _userService.UpdateUserInfo(info, email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("/user/info/get")]
        async public Task<IActionResult> GetUserInfo(string email)
        {
            UserInfo result = await _userService.GetUserInfo(email);
            return Json(result);
        }

        [Authorize]
        [HttpGet("/allspecialties/get")]
        async public Task<IActionResult> GetAllSpecialties()
        {
            var result = await _userService.GetAllSpecialties();
            return Json(result);
        }

        [Authorize]
        [HttpGet("/user/specialties/get")]
        async public Task<IActionResult> GetUserSpecialties(string email)
        {
            var result = await _userService.GetUserSpecialties(email);
            return Json(result);
        }

        [Authorize]
        [HttpPost("/user/specialties/update")]
        async public Task<IActionResult> UpdateUserSpecialties(UserSpecialties specialties, string email)
        {
            bool result = await _userService.UpdateUserSpecialties(specialties, email);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("/user/exams/update")]
        async public Task<IActionResult> UpdateUserExams(Exams exams, string email)
        {
            bool result = await _userService.UpdateUserExams(exams, email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("/user/exams/get")]
        async public Task<IActionResult> GetUserExams(string email)
        {
            Exams result = await _userService.GetUserExams(email);
            return Json(result);
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }



        [Authorize]
        [HttpGet("/test")]
        public IActionResult test()
        {
            var result = new
            {
                asd = "jh",
                gfd = "hkkbjn"
            };
            return Json(result);
        }
    }
}
