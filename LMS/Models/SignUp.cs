using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Models
{
    public class SignUp
    {
        [DisplayName("User ID")]
        public int id { get; set; }

        [DisplayName("Full Name")]
        public string name { get; set; }

        [DisplayName("E-mail")]
        public string email { get; set; }

        [DisplayName("Contact")]
        public string contact { get; set; }


        [DisplayName("Address")]
        public string address { get; set; }


        [DisplayName("Password")]
        
        public string password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string confirmPassword { get; set; }

        public SignUp(int id, string name, string email, string contact, string address, string password, string confirmPassword)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.contact = contact;
            this.address = address;
            this.password = password;
            this.confirmPassword = confirmPassword;
        }

        public SignUp(string name, string email, string contact, string address, string password, string confirmPassword)
        {
            this.name = name;
            this.email = email;
            this.contact = contact;
            this.address = address;
            this.password = password;
            this.confirmPassword = confirmPassword;
        }

        public SignUp(int id, string name, string email, string contact, string address, string password)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.contact = contact;
            this.address = address;
            this.password = password;
        }
    }
}