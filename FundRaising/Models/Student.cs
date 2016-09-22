using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

        [DisplayName("Student ID")]
        public string StudentID { get; set; }

        //[DisplayName("Student Name")]
        //public string StudentName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Teacher Name")]
        public string TeacherName { get; set; }

        
        public string Grade { get; set; }

       
        public string Password { get; set; }

     
        [NotMapped]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        
        public string Phone { get; set; }

        [DisplayName("School ID")]
        public string    SchoolID { get; set; }

        [DisplayName("Default Goal")]
        public string DefaultGoal { get; set; }

        [DisplayName("Image Url")]
        public string image { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }

        [DisplayName("Date Registered")]
        public Nullable<DateTime> DateRegistered { get; set; }

        [NotMapped]
        public string SchoolName { get; set; }

        [NotMapped]
        public string FullName { get; set; }

        [NotMapped]
        public string CampaignName { get; set; }

        [NotMapped]
        public double totalsales { get; set; }

        [NotMapped]
        public int totalOrders { get; set; }

        [NotMapped]
        public string CampaignDates { get; set; }

        [NotMapped]
        public string username { get; set; }

        [NotMapped]
       public string Status { get; set; }


    }
}