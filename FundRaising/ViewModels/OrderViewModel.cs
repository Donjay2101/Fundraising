using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class OrderViewModel
    {
        public int ID { get; set; }               
        public string FirstName { get; set; }
        public string LastName { get; set; }               
        public string CompanyName { get; set; }                                        
        public string StudentFirstName  { get; set; }
        public string StudentLastName { get; set; }
        public string  SchoolName { get; set; }
        public string SchoolID { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int ProductID { get; set;}
    }
}