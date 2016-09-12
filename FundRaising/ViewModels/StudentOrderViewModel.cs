using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class StudentOrderViewModel
    {


        public int OrderID { get; set; }
        public DateTime createdDate { get; set; }
        public string FirstName { get;set; }
        public string LastName { get; set; }
        public string DeliveryBy { get; set; }
        public double ProductTotal { get; set; }
        public  int Items{get;set;}
    }
}