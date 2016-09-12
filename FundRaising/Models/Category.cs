using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    [Table("Categories")]
    public class Category
    {
        public Category()
        {
            //this.Brochures = new HashSet<Brochure>();
        }

        #region Properties
        [Key]
        
        public int ID { get; set; }
        [Required(ErrorMessage = "*")]
        public string CategoryID { get; set; }
        [Required(ErrorMessage = "*")]
        public string CategoryName { get; set; }

        public string  Image { get; set; }

        [NotMapped]
        public string check{get;set;}
        //public virtual ICollection<Brochure> Brochures { get; set; }

        [NotMapped]
        public  ICollection<Product> products { get; set; }



        [NotMapped]
        public int  productsCount { get; set; }
        #endregion


    }
}