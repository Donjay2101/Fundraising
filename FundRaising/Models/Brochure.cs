using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    
    public class Brochure
    {

        #region Properties
        [Key]
        public int ID { get; set; }
        
        [Display(Name="Brochure ID")]
        [Required(ErrorMessage = "*")]
        public string BrochureID {get;set;}
        
        [DataType(DataType.MultilineText)]  
        public string Description { get; set; }
        
        [Display(Name = "Brochure Name")]
        [Required(ErrorMessage="*")]
        public string BrochureName { get; set; }
        
        
        //public virtual  ICollection<Category> Categories { get; set; }
      
        [NotMapped]
        public Product Products { get; set; }

        [NotMapped]
        public ICollection<Category> CategoryList{ get; set; }
        
        [NotMapped]
        public int productsCount { get; set; }
        #endregion




    }
}