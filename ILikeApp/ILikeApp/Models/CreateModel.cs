using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ILikeApp.Models
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class ViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string photo { get; set; }
        public int id { get; set; }
        public string access_token { get; set; }
    }
}