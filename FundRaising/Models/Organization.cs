using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FundRaising.Models
{
    [Authorize]
    public class Organization
    {
        #region Properties

       
        [Key]
        public int ID { get; set; }

        public int Distributor { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name="School ID")]
        public string  SchoolID{ get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "School Name")]
        public string Name { get; set; }

        [Display(Name = "School Contact Name")]
        [Required(ErrorMessage = "*")]
        public string ContactName { get; set; }

        [Display(Name = "School Address 1")]
        [Required(ErrorMessage = "*")]
        public string Address1 { get; set; }

        [Display(Name = "School Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "School City")]
        [Required(ErrorMessage = "*")]
        public string  City{ get; set; }

        [Display(Name = "School State")]
        [Required(ErrorMessage = "*")]
        public string State { get; set; }

        [Display(Name = "School Postal Code")]
        [Required(ErrorMessage = "*")]
        public string Postal { get; set; }

        //[Display(Name = "Organization Country")]
        //[Required(ErrorMessage = "*")]
        public string  Country{ get; set; }

        [Display(Name = "School Phone")]
        [Required(ErrorMessage = "*")]
        public string  Phone { get; set; }

        [Display(Name = "School Welcome")]        
        public string WelcomeMessage { get; set; }

        [Display(Name = "Primary Catalog")]
        [Required(ErrorMessage = "*")]
        public int Catalog { get; set; }

        public string _ShipToSchool;

        //public int ShiptoSchoolint
        //{
        //    get
        //    {
        //        return _ShipToSchool ; 
        //    }
        //    set
        //    {
        //        _ShipToSchool = value;
        //    }
        //}
        [Display(Name = "Allow Ship To School Items")]
        [Required(ErrorMessage = "*")]
        public bool ShipToSchool { get; set; }

        //public bool ShipToSchool  {
        //    get
        //    {
        //        return _ShipToSchool == "1" ? true : false;
        //    }
        //    set
        //    {
                
        //        _ShipToSchool = value?"1":"0";
        //    }
        //}

        [Display(Name = "Ship To School Only Option")]
        [Required(ErrorMessage = "*")]
        public bool ShioToSchoolOnly { get; set; }

        [Display(Name = "Ship To School Catalog")]
        [Required(ErrorMessage = "*")]
        public int ShipToSchoolCatalog  { get; set; }

        [Display(Name = "Email/Login ID")]
        [Required(ErrorMessage="*")]
        public string LoginID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        [Display(Name = "Participant Option")]
        [Required(ErrorMessage = "*")]
        public string ParticipantOption { get; set; }

        [Display(Name = "Pricing Level")]
        //[Required(ErrorMessage = "*")]
        public string PricingLevel { get; set; }

        //[Display(Name = "Free Shipping Amount")]
        //[Required(ErrorMessage = "*")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public bool FreeShippingAmount { get; set; }

        [Display(Name = "Free Shipping Amount")]
        //[Required(ErrorMessage = "*")]
        [NotMapped]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public string FreeShippingAmountS { get; set; }

        [Display(Name = "Auto Assign Participant ID?")]
        [Required(ErrorMessage = "*")]
        public bool AutoAssignParticipantID  { get; set; }

        [Display(Name = "Collect Grade/Teacher?")]
        [Required(ErrorMessage = "*")]
        public bool CollectTeacherGrade  { get; set; }

        [Display(Name = "Collect Cell Phone?")]
        
        public bool CollectCellPhone { get; set; }

        [Display(Name = "Cell Phone Required?")]
        
        public bool CellPhoneRequired { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Goal Type")]
        [Required(ErrorMessage = "*")]
        public string GoalType { get; set; }

        [Display(Name = "School Default Goal")]
        [Required(ErrorMessage = "*")]
        public decimal DefaultGoal { get; set; }
       
        [NotMapped]
        public RegisterModel DistributorObject { get; set; }

        [NotMapped]
        public List<Campaign> OrganizationCampaigns { get; set; }
        
        #endregion
    }
}
