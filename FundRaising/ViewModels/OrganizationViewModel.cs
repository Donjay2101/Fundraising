using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundRaising.ViewModels
{
    public class OrganizationViewModel
    {
        #region Properties


        public int ID { get; set; }

        public int Distributor { get; set; }

        public string SchoolID { get; set; }

        public string Name { get; set; }

        public string ContactName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Postal { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string WelcomeMessage { get; set; }

        public int Catalog { get; set; }

        public string _ShipToSchool;

        public bool ShipToSchool { get; set; }

       
        public bool ShioToSchoolOnly { get; set; }

        public int ShipToSchoolCatalog { get; set; }

        public string LoginID { get; set; }

        public string Password { get; set; }

        public string ParticipantOption { get; set; }

        public string PricingLevel { get; set; }

        public bool FreeShippingAmount { get; set; }

        public string FreeShippingAmountS { get; set; }

        public bool AutoAssignParticipantID { get; set; }

        public bool CollectTeacherGrade { get; set; }

       

        public bool CollectCellPhone { get; set; }

       

        public bool CellPhoneRequired { get; set; }

        public string GoalType { get; set; }

       
        public decimal DefaultGoal { get; set; }


        public string DistributorName { get; set; }


        //public List<Campaign> OrganizationCampaigns { get; set; }

        #endregion
    }
}