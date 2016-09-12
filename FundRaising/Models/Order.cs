using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FundRaising.ViewModels;

namespace FundRaising.Models
{
    public class Order
    {
        public int ID { get; set; }

        [Required(ErrorMessage="*")]
        [DisplayName("Email Address")]
        
        public string  EmailAddress { get; set; }

        [Required(ErrorMessage = "*")]
        public string Country { get; set; }

        [NotMapped]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "*")]
        //[DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Address 1")]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "*")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("State/Province")]
        public string State { get; set; }

        //[Required(ErrorMessage = "*")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

      
        [DisplayName("Country")]
        public string SCountry { get; set; }              
        
        [DisplayName("Company Name")]
        public string SCompanyName { get; set; }

        
        [DisplayName("Address 1")]
        public string SAddress1 { get; set; }
        
        [DisplayName("Address 2")]
        public string SAddress2 { get; set; }

        [DisplayName("City")]
        public string SCity { get; set; }

        [DisplayName("State/Province")]
        public string SState { get; set; }

        
        [DisplayName("Postal Code")]
        public string SPostalCode { get; set; }
        public string StudentID { get; set; }
        public int SchoolID { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CVVNumber{ get; set; }
        public string ExpirationDate { get; set; }
        public string ExpirationYear { get; set; }
        public string CardName { get; set; }
        public bool ShiptoSchool { get; set; }

        [NotMapped]
        public List<OrderDetail> OrderDetails { get; set; }

        [NotMapped]
        public List<Cart> OrderList { get; set; }

         [NotMapped]
        public OrderSummary OSummary{ get; set; }

    }
}