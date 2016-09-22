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
        public string SchoolID { get; set; }
        public bool IsActive{ get; set; }

        public bool Active { get; set; }

        public string FullName { get; set; }

        public string CampaignName { get; set; }

        public double totalsales { get; set; }

        public int totalOrders { get; set; }

        public string CampaignDates { get; set; }

        public string image { get; set; }

        public string username { get; set; }

        public string TeacherName { get; set; }

        public string Grade { get; set; }

        public string Status { get; set; }

        public Nullable<DateTime> DateRegistered { get; set; }
    }
}