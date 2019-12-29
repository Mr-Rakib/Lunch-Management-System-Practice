using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Models
{
   
    public class Login
    {
        [DisplayName("Username")]
        public string username { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string password { get; set; }

        public Login(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}