using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteLogiqPointOfSale
{
    public class CompletedOrder
    {
        public List<OrderItem> OrderItems { get; }
        public List<string> OrderCondiments { get; }
        public CompletedOrder(List<OrderItem> orderItems, List<string> condiments)
        {
            OrderItems = orderItems;
            OrderCondiments = condiments;
        }
    }
}
