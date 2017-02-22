using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ILikeApp.Models;

namespace ILikeApp.Infrastructure
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,
                                            IOwinContext context)
        {
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            //manager.PasswordValidator = new CustomPasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = false,
            //    RequireDigit = false,
            //    RequireLowercase = true,
            //    RequireUppercase = true
            //};

            //manager.UserValidator = new UserValidator<AppUser>(manager)
            //{
            //    AllowOnlyAlphanumericUserNames = true,
            //    RequireUniqueEmail = true
            //};

            //manager.UserValidator = new CustomUserValidator();

            return manager;
        }
    }
}