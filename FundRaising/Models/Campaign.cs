using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    [Table("Campaign")]    
    public class Campaign
    {

        #region Properties

        [Display(Name="CampaignID")]
        [Required(ErrorMessage = "*")]
        public int ID { get; set; }

       
        public string OrganizatonID { get; set; }
        
        [Display(Name="CampaignName")]
        [Required(ErrorMessage = "*")]
        public string CampaignName{get;set;}

        [Display(Name="CampaignStartDate")]
        [Required(ErrorMessage = "*")]
        public DateTime CampaignStartDate { get; set; }

        [Display(Name="CampaignEndDate")]
        [Required(ErrorMessage = "*")]
        public DateTime CampaignEndDate { get; set; }



        #endregion










    }
}