using ILikeApp.JsonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ILikeApp.Infrastructure
{
    public class VkontakteInfo
    {
        public static async Task<string> GetJsonAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync(url).Result;

                return response;
            }
        }
        
        public static async Task<Users> GetProfileInfo(string userId, string accessToken)
        {
            var json = await GetJsonAsync((String.Format("https://api.vk.com/method/users.get?user_ids={0}&fields=photo_50&access_token={1}&v=5.60", userId, accessToken)));
            var model = JsonConvert.DeserializeObject<Users>(json);
            return model;
        }

        public static async Task<Rootobject> GetFriend(string userId, string accessToken)
        {
            var json = await GetJsonAsync((String.Format("https://api.vk.com/method/friends.get?user_id={0}&fields=domain,photo_50&name_case=nom&access_token={1}&v=5.62", userId, accessToken)));
            var model = JsonConvert.DeserializeObject<Rootobject>(json);
            return model;
        }
    }
}