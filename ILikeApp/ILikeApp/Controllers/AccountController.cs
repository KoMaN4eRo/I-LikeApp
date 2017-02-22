using ILikeApp.Infrastructure;
using ILikeApp.JsonModels;
using ILikeApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ILikeApp.Infrastructure;

namespace ILikeApp.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Access Denied" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(details.Name,
                details.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password.");
                }
                else
                {
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, ident);
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(details);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Main", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VkontakteLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("VkontakteLoginCallback",
                new { returnUrl = returnUrl })
            };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Vkontakte");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> VkontakteLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            AppUser user = await UserManager.FindAsync(loginInfo.Login);

            var accesstoken = loginInfo.ExternalIdentity.FindFirst("VkontakteAccessToken");
            string json = await GetJsonAsync(String.Format("https://api.vk.com/method/users.get?user_ids=81959312&access_token={0}&v=5.60", accesstoken.Value));
            var model = JsonConvert.DeserializeObject<Users>(json);

            if (user == null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName
                };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }
            ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            ident.AddClaims(loginInfo.ExternalIdentity.Claims);
            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            },ident);
            //return View("MainPage", new ViewModel { first_name = model.response[0].first_name, last_name = model.response[0].last_name, IsAuthenticated = HttpContext.User.Identity.IsAuthenticated });
            return RedirectToAction("Main");
        }

        public async Task<ActionResult> MainPage()
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();

            return View(GetData("MainPage", loginInfo));
        }

        [AllowAnonymous]
        public ActionResult Main()
        {
            return PartialView();
        }

        public ActionResult UserCredits()
        {
            var model = GetProfileInfo();
            ExternalLoginInfo loginInfo = AuthManager.GetExternalLoginInfo();
            if (loginInfo != null)
                return PartialView(new ViewModel { first_name = model.Result.response[0].first_name, last_name = model.Result.response[0].last_name, photo = model.Result.response[0].photo_50, id = model.Result.response[0].id, access_token = loginInfo.ExternalIdentity.FindFirst("VkontakteAccessToken").Value });
            else 
                return PartialView();
        }

        public ActionResult PartialHeader()// Header for view HeaderLayout 
        {
            var model = GetProfileInfo();
            ExternalLoginInfo loginInfo = AuthManager.GetExternalLoginInfo();
            if (loginInfo != null)
                return PartialView(new ViewModel { first_name = model.Result.response[0].first_name, last_name = model.Result.response[0].last_name, photo = model.Result.response[0].photo_50, id = model.Result.response[0].id, access_token = loginInfo.ExternalIdentity.FindFirst("VkontakteAccessToken").Value });
            else
                return PartialView();
        }

        public async Task<ActionResult> GetFriendsData(string id) // Go to view? where user can set search options
        {
            ExternalLoginInfo logininfo = await AuthManager.GetExternalLoginInfoAsync();
            var json = await  VkontakteInfo.GetFriend(id, logininfo.ExternalIdentity.FindFirst("VkontakteAccessToken").Value);
            return View(json);
        }

        public async Task<Users> GetProfileInfo() 
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            var json = await GetJsonAsync(String.Format("https://api.vk.com/method/users.get?user_ids={0}&fields=photo_50&access_token={1}&v=5.60", loginInfo.Login.ProviderKey, loginInfo.ExternalIdentity.FindFirst("VkontakteAccessToken").Value));
            var model = JsonConvert.DeserializeObject<Users>(json);
            return model;
        }

        public async Task<ActionResult> GetUsersGroup()
        {

            return PartialView();
        }
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private Dictionary<string, object> GetData(string actionName, ExternalLoginInfo loginInfo)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Action", actionName);
            dict.Add("User", HttpContext.User.Identity.Name);
            dict.Add("Authenticated", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Auth Type", HttpContext.User.Identity.AuthenticationType);
            dict.Add("AccessToken", loginInfo.ExternalIdentity.FindFirst("VkontakteAccessToken"));
            return dict;
        }
        private string GetJson(Uri uri)
        {
            WebClient webClient = new WebClient();
            string response = webClient.DownloadString(uri);
            return response;
        }
        private async Task<string> GetJsonAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync(url).Result;
                
                return response;
            }
        }
    }
}