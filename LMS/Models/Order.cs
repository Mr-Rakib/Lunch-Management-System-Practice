using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Order
    {
        [DisplayName("User ID")]
        public int userID { get; set; }

        [DisplayName("Full Name")]
        public string name { get; set; }

        [DisplayName("Date")]

        public DateTime orderDate { get; set; }

        [DisplayName("Quantity")]
        public int quantity { get; set; }

        [DisplayName("Cost")]
        public float cost { get; set; }


        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime date1 { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime date2 { get; set; }

        public Order(int userID, DateTime date1, DateTime date2)
        {
            this.userID = userID;
            this.date1 = date1;
            this.date2 = date2;
        }

        public Order(int userID, string name, DateTime orderDate, int quantity, float cost)
        {
            this.userID = userID;
            this.name = name;
            this.orderDate = orderDate;
            this.quantity = quantity;
            this.cost = cost;
        }

        public Order(int userID, string name, DateTime orderDate, int quantity)
        {
            this.userID = userID;
            this.name = name;
            this.orderDate = orderDate;
            this.quantity = quantity;
        }

        public Order(int userID, string name, int quantity, float cost)
        {
            this.userID = userID;
            this.name = name;
            this.quantity = quantity;
            this.cost = cost;
        }
    }
}