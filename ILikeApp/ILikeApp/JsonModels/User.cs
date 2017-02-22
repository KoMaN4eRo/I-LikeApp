using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILikeApp.JsonModels
{
    public class Users
    {
        public User[] response { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string photo_50 { get; set; }
    }


        public class Rootobject
        {
            public Response response { get; set; }
        }

        public class Response
        {
            public int count { get; set; }
            public User[] items { get; set; }
        }
    
}