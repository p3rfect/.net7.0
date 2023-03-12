using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/token")]
        public IActionResult Token(string email, string password)
        {
            var identity = GetIdentity(email, password);
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

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            var connUser = _userService.GetUserByEmail(email);
            if (connUser.Email != null && connUser.Password == password)
            {
                var Claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, connUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, connUser.Role)
                };
                ClaimsIdentity identity = new ClaimsIdentity(Claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return identity;
            }
            return null;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
