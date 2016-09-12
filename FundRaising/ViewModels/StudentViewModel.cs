using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class StudentViewModel
    {

        public int ID { get; set;}
        public string StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string SchoolName { get; set; }
        public int SchoolID { get; set; }
        public bool IsActive{ get; set; }
    }
}