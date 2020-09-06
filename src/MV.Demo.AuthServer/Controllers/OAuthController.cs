using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MV.Demo.AuthServer.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(
            string response_type, // authorization flow type
            string client_id,     // client id
            string redirect_uri,  //
            string scope,         // what information I want e.g. email, tel...
            string state)         // random client-generated string to confirm that we are going back to the same client
        {
            // ?a=foo&b=bar
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);
            return View(model: query.ToString());
        }

        //TODO: make proper login form
        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirectUri,
            string state)
        {
            const string code = "BABABBABABABA";

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirectUri}{query}");
        }

        public async Task<IActionResult> Token(
            string grant_type,  // flow of access_token request
            string code,        // confirmation of the authentication process
            string redirect_uri,
            string client_id)
        {
            //TODO: validate the code

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

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject =
                new
                {
                    access_token,
                    token_type = "Bearer",
                    raw_claim = "oathTutorial" //can be anything
                };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, offset: 0, count: responseBytes.Length);

            return Redirect(redirect_uri);
        }
    }
}
