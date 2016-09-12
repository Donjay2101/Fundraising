using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace FundRaising.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DBConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    [Table("webpages_Roles")]
    public class Role
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public int RoleID { get; set; }
    }


    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    [Table("UserProfile")]
    public class RegisterModel
    {
       
        [Key]
        public int userID { get; set; }

        [Display(Name = "Company name")]
        [Required(ErrorMessage= "Company name required.")]       
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Username required.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "password required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password should match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Address1")]
        //[Required(ErrorMessage = "*")]
        public string Address1{ get; set; }


        [Display(Name = "Address2")]
        //[Required(ErrorMessage = "*")]
        public string Address2 { get; set; }


        [Display(Name = "City")]
        //[Required(ErrorMessage = "*")]
        public string City { get; set; }

        [Display(Name = "State")]
        //[Required(ErrorMessage = "*")]
        public string State { get; set; }


        [Display(Name = "Country")]
        //[Required(ErrorMessage = "*")]
        public string Country { get; set; }

        [Display(Name = "Email Address")]
        //[Required(ErrorMessage = "*")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        //[Required(ErrorMessage = "*")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        //[Required(ErrorMessage = "*")]
        public string FaxNumber { get; set; }

        [Display(Name = "User Enabled")]
        //[Required(ErrorMessage = "*")]
        public bool Enabled { get; set; }

        [Display(Name = "Pricing Level")]
        //[Required(ErrorMessage = "*")]
        public string PricingLevel { get; set; }

       

        [Display(Name = "Image")]
        //[Required(ErrorMessage = "*")]
        public string ImagePath { get; set; }

        [Display(Name = "Default Product Catalog Name")]
        //[Required(ErrorMessage = "*")]
        public string DefaultProductCatalog { get; set; }

        [Display(Name = "Ship To School Fee")]
        //[Required(ErrorMessage = "*")]
        public double ShipToSchoolFee { get; set; }

        [Display(Name = "Default S2S Catalog")]
        //[Required(ErrorMessage = "*")]
        public string DefaultSToSCatalog { get; set; }

        [Display(Name = "Default Participiant Option")]
        //[Required(ErrorMessage = "*")]
        public string ParticipantOption { get; set; }


        [Display(Name = "Default Free Shipment")]
        //[Required(ErrorMessage = "*")]
        public bool FreeShipment { get; set; }

        [Display(Name = "Default Goal Type")]
        //[Required(ErrorMessage = "*")]
        public string GoalType { get; set; }

        [Display(Name = "Default FAQ Catalog ID")]
        //[Required(ErrorMessage = "*")]
        public string FAQ { get; set; }

        [Display(Name = "DefaultShip To School ")]
        //[Required(ErrorMessage = "*")]
        public bool ShipToSchool { get; set; }

        [Display(Name = "Default Collect Grade/Teacher")]
        //[Required(ErrorMessage = "*")]
        public bool CollectTeacher{ get; set; }

        [Display(Name = "Default get Parent Certification")]
        //[Required(ErrorMessage = "*")]
        public bool ParentCertification { get; set; }

      

        [Display(Name = "Collect Student Address")]
        //[Required(ErrorMessage = "*")]
        public bool StudentAddress { get; set; }

        [Display(Name = "Parent Required")]
        //[Required(ErrorMessage = "*")]
        public bool ParentRequired { get; set; }

        [Display(Name = "Collect Cell Phone")]
        //[Required(ErrorMessage = "*")]
        public bool CellPhone { get; set; }

        [Display(Name = "Cell Phone Required")]
        //[Required(ErrorMessage = "*")]
        public bool CellPhoneRequired { get; set; }

        [Display(Name = "Default Organization Goal")]
        [Required(ErrorMessage = "*")]
        public int OrganzationGoal { get; set; }

        [Display(Name = "Default EventID")]
        //[Required(ErrorMessage = "*")]
        public string EventID { get; set; }

        [Display(Name = "Default event Type ID")]
        //[Required(ErrorMessage = "*")]
        public string EventTypeID { get; set; }

        [Display(Name = "Default want to participate Cost")]
        //[Required(ErrorMessage = "*")]
        public string ParticipateCost { get; set; }

        [Display(Name = "Auto Assign Participant ID")]
        public bool AutoAssignParticipantID { get; set; }

        
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
