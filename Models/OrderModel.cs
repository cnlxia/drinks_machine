using Drinks_Machine_Program.Entities;
using Drinks_Machine_Program.Entities.Coins;
using Drinks_Machine_Program.Entities.Items;
using Drinks_Machine_Program.Entities.Stock;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Models
{
    public class OrderModel
    {
        private readonly ItemStock _ItemStock;
        private readonly CoinStock _CoinStock;

        public OrderModel(ItemStock _ItemStock, CoinStock _CoinStock)
        {
            this._ItemStock = _ItemStock;
            this._CoinStock = _CoinStock;
        }

        public List<SelectListItem> GetSelectListItems()
        {
            var SelectListItems = new List<SelectListItem>();
            var ListItems = _ItemStock.GetItems();

            foreach (var Item in ListItems)
            {
                SelectListItems.Add(new SelectListItem { Text = Item.Name, Selected = true });
            }

            return SelectListItems;
        }

        public List<SelectListItem> GetSelectListCoins()
        {
            var SelectListCoins = new List<SelectListItem>();
            var ListCoins = _CoinStock.GetCoins();

            foreach (var Coin in ListCoins)
            {
                SelectListCoins.Add(new SelectListItem { Text = Coin.Name, Selected = true });
            }

            return SelectListCoins;
        }

        public bool IsAnyItemSelected(List<SelectListItem> selectedItems)
        {
            if (selectedItems != null)
            {
                return selectedItems.Any(item => item.Value != null);
            }
            return false;
        }

        public bool IsAnyCoinUsed(List<SelectListItem> usedCoins)
        {
            if (usedCoins != null)
            {
                return usedCoins.Any(coin => coin.Value != null);
            }
            return false;
        }

        public bool IsEnoughMoney(List<SelectListItem> usedCoins, int totalCost)
        {
            var totalPaid = GetTotalPaid(usedCoins);
            return totalPaid >= totalCost;
        }

        public int GetTotalPaid(List<SelectListItem> usedCoins)
        {
            var coinsList = _CoinStock.GetCoins();

            int totalPaid = 0;
            if (usedCoins != null)
            {
                foreach (var coin in usedCoins)
                {
                    if (coin.Value != null)
                    {
                        var coinInserted = coinsList.FirstOrDefault(c => c.Name == coin.Text);

                        if (coinInserted != null)
                        {
                            int coinAmount = Convert.ToInt32(coin.Value);
                            totalPaid += coinInserted.Value * coinAmount;
                        }
                    }
                }
            }
            return totalPaid;
        }

        public Receipt GenerateReceipt(List<SelectListItem> CoinsUsed, List<SelectListItem> ItemsBought, int Cost)
        {
            var receipt = new Receipt();
            receipt.TotalCost = Cost;
            receipt.ItemsBought = ParseItem(ItemsBought);
            receipt.TotalPaid = GetTotalPaid(CoinsUsed);
            receipt.IsEnoughItems = IsEnoughItems(receipt.ItemsBought);

            var change = receipt.TotalPaid - Cost;
            receipt.Change = GetChangeBreakDown(change);
            receipt.IsEnoughChange = IsEnoughChange(change);

            return receipt;
        }

        public List<Item> ParseItem(List<SelectListItem> ItemsBought)
        {
            var itemsList = new List<Item>();
            foreach (var item in ItemsBought)
            {
                var itemInstance = GetItemInstance(item.Text);

                if (itemInstance != null)
                {
                    if (item.Value != null)
                    {
                        itemInstance.Name = item.Text;
                        itemInstance.Quantity = Convert.ToInt32(item.Value);
                        itemsList.Add(itemInstance);
                    }
                }
            }

            return itemsList;
        }

        public Item GetItemInstance(string name)
        {
            Item instance = null;

            if (name == "Coke")
            {
                instance = new Coke();
            }
            else if (name == "Pepsi")
            {
                instance = new Pepsi();
            }
            else if (name == "Soda")
            {
                instance = new Soda();
            }

            return instance;
        }

        public bool IsEnoughItems(List<Item> ItemsBought)
        {
            var itemsList = _ItemStock.GetItems();
            foreach (var item in itemsList)
            {
                var boughtItemFound = ItemsBought.FirstOrDefault(i => i.Name == item.Name);
                if (boughtItemFound != null)
                {
                    if (item.Quantity - boughtItemFound.Quantity < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Dictionary<Coin.Coins, int> GetChangeBreakDown(int change)
        {
            var coinsList = _CoinStock.GetCoins();
            coinsList.Sort((x, y) => { return y.Value.CompareTo(x.Value); });

            var changeBreakDown = new Dictionary<Coin.Coins, int>();
            foreach (var coin in coinsList)
            {
                Coin.Coins enumCoin;
                if (Enum.TryParse(coin.Name, out enumCoin))
                {
                    var changeAmount = (int)(change / coin.Value);
                    change = change % coin.Value;

                    if (changeAmount > 0)
                    {
                        changeBreakDown.Add(enumCoin, changeAmount);
                    }
                }
            }

            return changeBreakDown;
        }

        public bool IsEnoughChange(int change)
        {
            var coinsList = _CoinStock.GetCoins();
            var changeBreakDown = GetChangeBreakDown(change);

            foreach (var coin in coinsList)
            {
                Coin.Coins enumCoin;
                if (Enum.TryParse(coin.Name, out enumCoin))
                {
                    if (changeBreakDown.ContainsKey(enumCoin))
                    {
                        int coinReturnVal;
                        if (changeBreakDown.TryGetValue(enumCoin, out coinReturnVal))
                        {
                            if (coin.Amount < coinReturnVal)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;

        }

        public List<Item> GetUpdatedItemStock(List<Item> ItemsBought)
        {
            var itemsList = new List<Item>(_ItemStock.GetItems());
            foreach (var item in itemsList)
            {
                var ItemsBoughtFound = ItemsBought.FirstOrDefault(i => i.Name == item.Name);
                if (ItemsBoughtFound != null)
                {
                    item.Quantity = item.Quantity - ItemsBoughtFound.Quantity;
                }
            }
            return itemsList;
        }

        public List<Coin> GetUpdatedCoinStock(Dictionary<Coin.Coins, int> change)
        {
            var coinsList = new List<Coin>(_CoinStock.GetCoins());
            foreach (var coin in coinsList)
            {
                Coin.Coins enumCoin;
                if (Enum.TryParse(coin.Name, out enumCoin))
                {
                    if (change.ContainsKey(enumCoin))
                    {
                        int coinReturnVal;
                        if (change.TryGetValue(enumCoin, out coinReturnVal))
                        {
                            coin.Amount = coin.Amount - coinReturnVal;
                        }
                    }
                }
            }
            return coinsList;
        }

        public List<string> GetReceiptMessage(Receipt receipt)
        {
            var messages = new List<string>();
            if (receipt != null)
            {
                if (!receipt.IsEnoughItems)
                {
                    messages.Add("Drink is sold out, your purchase cannot be processed!");
                }
                else if (!receipt.IsEnoughChange)
                {
                    messages.Add("Not sufficient change in the inventory!");
                }
                else
                {
                    messages.AddRange(GetSuccessfullReceipt(receipt));
                }
            }
            return messages;
        }

        public List<string> GetSuccessfullReceipt(Receipt receipt)
        {
            var message = new List<string>();
            message.Add("Drinks Bought: \n");
            foreach (var item in receipt.ItemsBought)
            {
                message.Add($"{item.Name} Quantity: {item.Quantity}\n");
            }
            message.Add("\n\n");
            message.Add("Total Cost: \n");
            message.Add("$" + $"{receipt.TotalCost/100.0}\n");
            message.Add("\n\n");

            return message;
        }

    }
}
