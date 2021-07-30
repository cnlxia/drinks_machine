using Drinks_Machine_Program.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks_Machine_Program.Forms
{
    public class DrinksMachineForm
    {
        public DrinksMachineModel DrinksMachineModel { get; set; }

        public List<SelectListItem> ItemsBought { get; set; }

        public List<SelectListItem> CoinsUsed { get; set; }

        public List<string> Messages { get; set; }

        public int Total { get; set; }

        public DrinksMachineForm(DrinksMachineModel DrinksMachineModel)
        {
            this.DrinksMachineModel = DrinksMachineModel;
            this.ItemsBought = new List<SelectListItem>();
            this.CoinsUsed = new List<SelectListItem>();
            this.Messages = new List<string>();
            this.Total = 0;
        }

        public DrinksMachineForm()
        {
            this.ItemsBought = new List<SelectListItem>();
            this.CoinsUsed = new List<SelectListItem>();
            this.Messages = new List<string>();
            this.Total = 0;
        }
    }
}
