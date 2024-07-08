using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteLogiqPointOfSale
{
    public class OrderItem
    {
        public MenuItem Item { get; }
        public string Size { get; }

        public OrderItem(MenuItem item, string size) {
            Item = item;
            Size = size;
        }
    }
}
