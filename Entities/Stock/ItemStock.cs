using Drinks_Machine_Program.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Entities.Stock
{
    public class ItemStock
    {
        private List<Item> Items;

        public ItemStock()
        {
            Items = new List<Item>
            {
                new Coke
                {
                    ID = 1, Name = "Coke", Quantity = 5, Cost = 25
                },
                new Pepsi
                {
                    ID = 2, Name = "Pepsi", Quantity = 15, Cost = 36
                },
                new Soda
                {
                    ID = 3, Name = "Soda", Quantity = 3, Cost = 45
                }
            };
        }

        public List<Item> GetItems()
        {
            return Items;
        }

        public void SetItems(List<Item> Items)
        {
            this.Items = new List<Item>(Items); ;
        }
    }
}
