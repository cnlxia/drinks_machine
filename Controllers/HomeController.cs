using Drinks_Machine_Program.Entities;
using Drinks_Machine_Program.Entities.Stock;
using Drinks_Machine_Program.Forms;
using Drinks_Machine_Program.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DrinksMachineForm _DrinksMachineForm;
        private readonly VendingMachineModel _VendingMachineModel;
        private readonly OrderModel _OrderModel;
        private readonly ItemStock _ItemStock;
        private readonly CoinStock _CoinStock;
        private Receipt _Receipt;

        public HomeController(ILogger<HomeController> logger, DrinksMachineForm _DrinksMachineForm, VendingMachineModel _VendingMachineModel, OrderModel _OrderModel, ItemStock _ItemStock, CoinStock _CoinStock)
        {
            _logger = logger;
            this._DrinksMachineForm = _DrinksMachineForm;
            this._VendingMachineModel = _VendingMachineModel;
            this._OrderModel = _OrderModel;
            this._ItemStock = _ItemStock;
            this._CoinStock = _CoinStock;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                SetInitialFormValue();

                return View(_DrinksMachineForm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetInitialFormValue()
        {
            this._VendingMachineModel.ItemsStock = _ItemStock.GetItems();
            this._VendingMachineModel.CoinsStock = _CoinStock.GetCoins();
            this._DrinksMachineForm.ItemsBought = _OrderModel.GetSelectListItems();
            this._DrinksMachineForm.CoinsUsed = _OrderModel.GetSelectListCoins();
            this._DrinksMachineForm.DrinksMachineModel = (DrinksMachineModel)_VendingMachineModel;
        }

        [HttpPost]
        public IActionResult Index(DrinksMachineForm form)
        {
            try
            {
                _DrinksMachineForm.Messages = new List<string>();

                if (!_OrderModel.IsAnyItemSelected(form.ItemsBought))
                {
                    _DrinksMachineForm.Messages.Add("No item is selected!");
                }

                if (!_OrderModel.IsAnyCoinUsed(form.CoinsUsed))
                {
                    _DrinksMachineForm.Messages.Add("No coin is inserted!");
                }

                if (!_OrderModel.IsEnoughMoney(form.CoinsUsed, form.Total))
                {
                    _DrinksMachineForm.Messages.Add("Not enough money for the purchase!");
                }

                if (_DrinksMachineForm.Messages.Count > 0)
                {
                    SetInitialFormValue();
                    return View(_DrinksMachineForm);
                }

                _Receipt = _OrderModel.GenerateReceipt(form.CoinsUsed, form.ItemsBought, form.Total);
                if (_Receipt.IsEnoughItems && _Receipt.IsEnoughChange)
                {
                    var updatedItemStock = _OrderModel.GetUpdatedItemStock(_Receipt.ItemsBought);
                    var updatedCoinStock = _OrderModel.GetUpdatedCoinStock(_Receipt.Change);
                    _ItemStock.SetItems(updatedItemStock);
                    _CoinStock.SetCoins(updatedCoinStock);
                }

                _DrinksMachineForm.Messages = _OrderModel.GetReceiptMessage(_Receipt);
                ModelState.Clear();

                SetInitialFormValue();
                return View(_DrinksMachineForm);
            }
            catch (Exception)
            {
                SetInitialFormValue();
                return View("Index", _DrinksMachineForm);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
