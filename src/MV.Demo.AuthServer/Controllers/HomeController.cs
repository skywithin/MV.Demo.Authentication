using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MV.Demo.AuthServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie"),
            };

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret));
            var signingCresentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    Constants.Issuer,
                    Constants.Audience,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCresentials
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }

        public IActionResult Decode(string tokenPart)
        {
            var bytes = Convert.FromBase64String(tokenPart);
            var jsonString = Encoding.UTF8.GetString(bytes);
            return Ok(jsonString);
        }
    }
}
