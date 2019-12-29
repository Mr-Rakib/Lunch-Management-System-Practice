using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class User
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

        public User(int id, string name, string email, string contact, string address)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.contact = contact;
            this.address = address;
        }
    }
}