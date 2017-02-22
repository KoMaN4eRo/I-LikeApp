using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILikeApp.Models
{
    public class Friends
    {
        public List<UserInfo> users;
    }

    public class UserInfo
    {
        public string photo_50;
        public string first_name;
        public string last_name;
        public Dictionary<int, int> wall;
        public Dictionary<int, int> photo;
    }
}
