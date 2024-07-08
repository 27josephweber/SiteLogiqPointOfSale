using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteLogiqPointOfSale
{
    public class PointOfSale
    {
        public List<MenuItem> Menu { get; }
        public List<OrderItem>  OrderItems { get { return _orderItems; } }
        private List<OrderItem> _orderItems;
        
        public Double Cash { get { return _cash; } }
        private Double _cash;
        public Double OrderTotal { get { return _orderTotal; } }
        private Double _orderTotal;
        public List<string>  AvailableCondiments { get; }
        public List <string> OrderCondiments { get { return _orderCondiments; } }
        private List<string> _orderCondiments;

        private List<int> condimentCounts;

        private readonly List<Double> acceptedCashDenominations = new List<double>
        {
            .05,
            .10,
            .25,
            1.00,
            5.00,
            10.00,
            20.00
        };
        private readonly double mediumAdjustment = .50;
        private readonly double largeAdjustment = 1.00;
        private readonly double condimentPrice = .25;


        public PointOfSale() {
            Menu = new List<MenuItem>();
            Menu.Add(new MenuItem("hot dog", 5.00));
            Menu.Add(new MenuItem("hamburger", 8.00));
            Menu.Add(new MenuItem("fries", 2.00));
            Menu.Add(new MenuItem("italian beef", 9.00));
            Menu.Add(new MenuItem("gyro", 12.00));
            AvailableCondiments = new List<string>
            {
                "ketchup",
                "mustard"
            };
            resetOrder();
            _cash = 0.0;


        }

        
        /*
         * reinitilizes the order, but keeps the current cash in the POS
         * 
         */
        private void resetOrder()
        {
            _orderTotal = 0.0;
            _orderItems = new List<OrderItem>();
            _orderCondiments = new List<string>();
            condimentCounts = new List<int>(AvailableCondiments.Count);
            for(int i = 0; i < AvailableCondiments.Count; i++)
            {
                condimentCounts.Add(0);
            }
        }

        public CompletedOrder Serve()
        {
            if (_orderTotal == 0.0)
            {
                throw new InvalidOperationException("There are no items in this order");
            }
            if (_orderTotal > _cash)
            {
                throw new InvalidOperationException($"Current cash,{_cash}, is insufficient for this order of {_orderTotal}");
            }
            CompletedOrder completedOrder = new CompletedOrder(_orderItems, _orderCondiments);
            _cash = _cash - _orderTotal;
            resetOrder();


            return completedOrder;
        }
        public Double EndTransaction()
        {
            double change = _cash;
            resetOrder();
            _cash = 0.0;
            return change;
        }
        public void AddCash(double amount)
        {
            if(acceptedCashDenominations.Contains(amount))
            {
                this._cash += amount;
            }
            else
            {
                throw new ArgumentException($"{amount} is not a valid denomination of cash");
            }

        }
        public void AddItem(string item, string size) {
            item = item.ToLower();
            foreach(MenuItem menuItem in Menu){
                //we are not currently worried about duplicate menu items
                if(item == menuItem.Name)
                {
                    size = size.ToLower();
                    double price = menuItem.Price;
                    switch (size)
                    {
                        case "small":
                            _orderTotal += price;
                            break;
                        case "medium":
                            _orderTotal += price + mediumAdjustment;
                            break;
                        case "large":
                            _orderTotal += price + largeAdjustment;
                            break;
                        default:
                            throw new ArgumentException($"{size} is not a valid size");
                    }
                    _orderItems.Add(new OrderItem(menuItem, size));
                    return;
                }
            }
            throw new ArgumentException($"{item} is not a valid menu item");
        }
        public void AddCondiment(string condiment) {
            condiment = condiment.ToLower();
            for(int i = 0; i < AvailableCondiments.Count; i++)
            {
                if (condiment == AvailableCondiments[i])
                {
                    if (condimentCounts[i] >= 3)
                    {
                        throw new InvalidOperationException($"The maximum number of {condiment} has already been ordered");
                    }

                    condimentCounts[i] += 1;
                    _orderCondiments.Add(condiment);
                    _orderTotal += condimentPrice;
                    return;

                }
            }
            throw new ArgumentException($"{condiment} is not an available condiment");
        }

    }
}
