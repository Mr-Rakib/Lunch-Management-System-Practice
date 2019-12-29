using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class OrderViewModel
    {
        private PagedList<Order> pagedList;

        public List<Order> orederList { get; set; }
        public List<Order> allOrederList { get; set; }
        public Order myOrder { get; set; }
        public Order order { get; set; }
        public List<Order> perHeadCost { get; set; }

        public IPagedList<Order> AllOrder { get; set; }

        public OrderViewModel( IPagedList<Order> allOrder, Order order)
        {
            
            this.AllOrder = allOrder;
            this.order = order;
        }

        public OrderViewModel(List<Order> orederList, List<Order> allOrederList, Order myOrder, List<Order> perHeadCost) : this(orederList, allOrederList, myOrder)
        {
            this.perHeadCost = perHeadCost;
        }

        public OrderViewModel(List<Order> orederList, List<Order> allOrederList, Order myOrder)
        {
            this.orederList = orederList;
            this.allOrederList = allOrederList;
            this.myOrder = myOrder;
        }

        public OrderViewModel(List<Order> allOrederList, Order order)
        {
            this.allOrederList = allOrederList;
            this.order = order;
        }

        
    }
}