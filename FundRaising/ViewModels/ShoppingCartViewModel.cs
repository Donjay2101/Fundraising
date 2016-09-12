using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FundRaising.Models;

namespace FundRaising.ViewModels
{
    public class ShoppingCartViewModel
    {

        public List<Cart> cartItems { get; set; }
        public double  CartTotal { get; set; }
        public int Count { get; set; }
        public bool IsSalesTaxChargable { get; set; }
    }
}