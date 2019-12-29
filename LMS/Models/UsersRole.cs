using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class UsersRole
    {
        [DisplayName("User ID")]
        public int userID { get; set; }

        [DisplayName("Full Name")]
        public string fullName { get; set; }

        [DisplayName("Username")]
        public string username { get; set; }

        [DisplayName("Current Role")]
        public string userCurrentRole { get; set; }

        [DisplayName("User Role")]
        public string userRole { get; set; }

        public UsersRole(int userID, string fullName, string username, string userCurrentRole, string userRole)
        {
            this.userID = userID;
            this.fullName = fullName;
            this.username = username;
            this.userCurrentRole = userCurrentRole;
            this.userRole = userRole;
        }

        public UsersRole(int userID, string fullName, string username, string userRole)
        {
            this.userID = userID;
            this.fullName = fullName;
            this.username = username;
            this.userRole = userRole;
        }
    }

    public enum Roles
    {
        admin,
        subadmin,
        user,
        none
        
    }

   

}