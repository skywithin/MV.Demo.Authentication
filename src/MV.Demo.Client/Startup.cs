using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace MV.Demo.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
                    {
                        // 1. What we use for authentication.
                        //    We check the cookie to confirm that we are authenticated
                        config.DefaultAuthenticateScheme = Constants.AuthenticationScheme;

                        // 2. What we do when we sign in.
                        //    When we sign in, we will deal out a cookie
                        config.DefaultSignInScheme = Constants.AuthenticationScheme;

                        // 3. How to check if we are allowed to do something
                        //    Use this to check if we are allowed to do something
                        //    Go via our server to recieve a client cookie
                        config.DefaultChallengeScheme = Constants.ChallengeScheme;
                    })
                    .AddCookie(Constants.AuthenticationScheme)
                    .AddOAuth(Constants.ChallengeScheme, config =>
                    {
                        config.ClientId = Constants.ClientId;
                        config.ClientSecret = Constants.ClientSecret;
                        
                        config.AuthorizationEndpoint =  Constants.AuthServerAuthorizationEndpoint;
                        config.CallbackPath = Constants.ClientAuthCallbackPath;
                        config.TokenEndpoint =  Constants.AuthServerTokenEndpoint;

                        config.SaveTokens = true; // Make sure access_token is saved

                        //Load claims into our cookie
                        config.Events =
                            new OAuthEvents
                            {
                                OnCreatingTicket = (context) =>
                                {
                                    var accessToken = context.AccessToken;
                                    var base64Payload = accessToken.Split('.')[1];
                                    var bytes = Convert.FromBase64String(base64Payload);
                                    var jsonPayload = Encoding.UTF8.GetString(bytes);
                                    var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

                                    foreach (var claim in claims)
                                    {
                                        context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                                    }
                                    return Task.CompletedTask;
                                }
                            };
                    });

            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
