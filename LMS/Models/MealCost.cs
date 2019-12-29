using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class MealCost
    {

        public DateTime modifyDate { get; set; }
        public int costPerMeal { get; set; }

        public MealCost(DateTime modifyDate, int costPerMeal)
        {
            this.modifyDate = modifyDate;
            this.costPerMeal = costPerMeal;
        }
    }
}