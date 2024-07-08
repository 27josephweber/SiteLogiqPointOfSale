using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteLogiqPointOfSale
{
    public class MenuItem
    {
        public string Name { get; }
        public Double Price { get; }

        public MenuItem(string name, Double price){
            Name = name;
            Price = price;
        }


    }
}
