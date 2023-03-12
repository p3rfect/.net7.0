using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication4.jwt
{
    public class AuthOptions
    {
        public const string ISSUER="practice";
        public const string AUDIENCE = "client";
        private const string _KEY ="qwertyuiopasdfghjklzxcvbnm123";
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_KEY));

    }
}
