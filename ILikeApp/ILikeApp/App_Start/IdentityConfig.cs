using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Security.Claims;
using Duke.Owin.VkontakteMiddleware;
using Duke.Owin.VkontakteMiddleware.Provider;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.Infrastructure;
using System.Security.Principal;
using System.Collections.Concurrent;
using Microsoft.Owin.Security;
using ILikeApp.Infrastructure;

namespace ILikeApp
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            //app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            var googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "67606520237-5vvmm7gsu7s0d6kiuvuf6vcbqmkpd1md.apps.googleusercontent.com",
                ClientSecret = "vx1SwjY4hZC6W6fzS8oVOuUl",
            };
            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);


            var vkontakteAuthentication = new VkAuthenticationOptions
            {
                AppId = "5687489",
                AppSecret = "ZGkOJrq3VCXzADho9Leh",
                Scope = "friends, status, wall, groups, photo",
                Provider = new VkAuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                    {

                        context.Identity.AddClaim(new System.Security.Claims.Claim("VkontakteAccessToken", context.AccessToken));
                        

                        // Retrieve the OAuth access token to store for subsequent API calls
                        //var accessToken = context.AccessToken;
                        // Retrieve the username
                        //var vkontakteName = context.UserName;
                        //return Task.FromResult(0);
                    }
                }
            };
            vkontakteAuthentication.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            app.UseVkontakteAuthentication(vkontakteAuthentication);
        }
    }
}