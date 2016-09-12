using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    [Table("MapCategory")]
    public class MapCategory
    {
        public int ID { get; set; }
        public int BrochureID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }

    }


    //[Table("MapBrochureCategory")]
    //public class MapBrochureCategory
    //{
      
    //    public int BorchureID { get; set; }
    //    public int CategoryID { get; set; }
    //   // public int ProductID { get; set; }

    //}


    //[Table("MapCategoryProducts")]
    //public class MapCategoryProducts
    //{

    //    public int CategoryID { get; set; }
    //    public int ProductID { get; set; }
    //    // public int ProductID { get; set; }

    //}
}