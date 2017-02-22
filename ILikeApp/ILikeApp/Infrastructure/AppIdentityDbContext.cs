using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ILikeApp.Models;
using Microsoft.AspNet.Identity;

namespace ILikeApp.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext() : base("LikeDb") { }
        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }
        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }
    public class IdentityDbInit : NullDatabaseInitializer<AppIdentityDbContext>
    {
    }
}
