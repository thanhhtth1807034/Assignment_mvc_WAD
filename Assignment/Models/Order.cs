using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public OrderStatus Status { get; set; }

        public enum OrderStatus { Pending = 5, Confirmed = 4, Shipping = 3, Paid = 2, Done = 1, Cancel = 0, Deleted = -1 }
        
        public Order()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = OrderStatus.Pending;
        }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}