using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class SalesTaxCharge
    {
        public int ID { get; set; }
        public string State { get;set;}
        public double TaxAmount { get; set; }
        public bool Active { get;set;}
        [NotMapped]
        public string Checked{get;set;}
    }
}