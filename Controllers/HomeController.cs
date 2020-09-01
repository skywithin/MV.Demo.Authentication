using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MV.Demo.Authentication.Controllers
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
            var grandmaClaims = 
                new[]
                {
                    new Claim(ClaimTypes.Name, "Max"),
                    new Claim(ClaimTypes.Email, "max@fmail.com"),
                    new Claim("Grandma.Says", "Max is a good boy"),
                };

            var licenseClaims =
                new[]
                {
                    new Claim(ClaimTypes.Name, "Max V Foo"),
                    new Claim("DrivingLicense", "A+"),
                };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
