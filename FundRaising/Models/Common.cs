using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundRaising.Models
{
    public class Common
    {


        public static List<ShipAmount> getShipAmountList()
        {
            List<ShipAmount> ship = new List<ShipAmount>() { new ShipAmount { ID = "$60.00", Text = "$60.00" }, 
                                                             new ShipAmount { ID = "$70.00", Text = "$70.00" },
                                                             new ShipAmount { ID = "$75.00", Text = "$75.00" } , 
                                                            new ShipAmount { ID = "$80.00", Text = "$80.00" },
                                                            new ShipAmount { ID = "$90.00", Text = "$90.00" },
                                                                new ShipAmount { ID = "$100.00", Text = "$100.00" }};
            return ship;

        }

        public static List<PricingLevel> getPricingLevel()
        {
            List<PricingLevel> level = new List<PricingLevel>() { new PricingLevel{ ID = "PricingLevelA", Text = "Pricing Level A" }, 
                                                             new PricingLevel{ ID = "PricingLevelB", Text = "Pricing Level B" },
                                                             new PricingLevel{ ID = "PricingLevelC", Text = "Pricing Level C" }   
                                                           };
            return level;

        }



        public static List<ParticipantOption> getParticipantOption()
        {
            List<ParticipantOption> level = new List<ParticipantOption>() { new ParticipantOption{ ID = "NameANDEmailOnly", Text = "A- Name and Email Only" }, 
                                                             new ParticipantOption{ ID = "NameEmailAndPersonalGreeting", Text = "B- Name,Email and Personal Greeting" },
                                                             new ParticipantOption{ ID = "NameEmailandPersonalGreetingandPhoto", Text = "C- Name,Email,Personal Greeting and Photo" }   
                                                           };
            return level;

        }


        public static List<FAQ> getFAQList()
        {
            List<FAQ> level = new List<FAQ>() { new FAQ{ ID = "GeneralQuestions", Text = "GeneralQuestions" }};
            return level;

        }


        public static List<Catalog> GetCatalog()
        {
            List<Catalog> catalog = new List<Catalog>(){new Catalog(){ID="ACADEMY OF NOLA DUNN",Text="ACADEMY OF NOLA DUNN"},
                                    new Catalog(){ID="ALEDO PTO",Text="ALEDO PTO"},
            new Catalog(){ID="BK SHIP TO HOME",Text="BK SHIP TO HOME" },
            new Catalog(){ID="CHARACTER - SHIP TO HOME",Text="CHARACTER - SHIP TO HOME"},
            new Catalog(){ID="CHARACTER - SHIP TO SCHOOL",Text="CHARACTER - SHIP TO SCHOOL"},
            new Catalog(){ID="CHARACTER - SHIP TO SCHOOL3",Text="CHARACTER - SHIP TO SCHOOL3"}};

            //List<SelectListItem> catalog = new List<SelectListItem>();
            //catalog.Add(new SelectListItem() { Text = "ACADEMY OF NOLA DUNN", Value = "ACADEMY OF NOLA DUNN" });
            //catalog.Add(new SelectListItem() { Text = "ALEDO PTO", Value = "ALEDO PTO" });
            //catalog.Add(new SelectListItem() { Text = "BK SHIP TO HOME", Value = "BK SHIP TO HOME" });
            //catalog.Add(new SelectListItem() { Text = "CHARACTER - SHIP TO HOME", Value = "CHARACTER - SHIP TO HOME" });
            //catalog.Add(new SelectListItem() { Text = "CHARACTER - SHIP TO SCHOOL", Value = "CHARACTER - SHIP TO SCHOOL" });
            //catalog.Add(new SelectListItem() { Text = "CHARACTER - SHIP TO SCHOOL3", Value = "CHARACTER - SHIP TO SCHOOL3" });


            return catalog;
        }

        public static List<GoalType> GoalType()
        {

            List<GoalType> GoalType = new List<GoalType>() { new GoalType() { ID = "0", Text = "Item" }, new GoalType() { ID = "1", Text = "Money" } };

            //List<SelectListItem> GoalType= new List<SelectListItem>();

            //GoalType.Add(new SelectListItem() { Text = "False", Value = "0" });
            //GoalType.Add(new SelectListItem() { Text = "True", Value = "1" });

            return GoalType;
        }


        public static List<SelectListItem> BoolOption()
        {
        //    List<BoolValue> boolList = new List<BoolValue>() { new BoolValue(){ID="0",Text="False"},new BoolValue(){ID="1",Text="True"}};


            List<SelectListItem> boollist = new List<SelectListItem>();

            boollist.Add(new SelectListItem() { Text = "True", Value = "0" });
            boollist.Add(new SelectListItem() { Text = "1", Value = "1" });

            return boollist;
        }

     
        public static List<Country> GetCountries()
        {

            List<Country> countries = new List<Country>() { new Country(){ID="United states",Text="United States"}};

            //List<SelectListItem> countries = new List<SelectListItem>();

            //countries.Add(new SelectListItem { Text = "Select", Value = "Select" });
            //countries.Add(new SelectListItem { Text = "United States", Value = "United States" });


            return countries;
        }
        public static List<State> GetStates()
        {
            List<State> states = new List<State>(){new State(){ID="Alabama",Text="Alabama"},
new State(){ID="Alabama",Text="Alabama"},
new State(){ID="Alaska",Text="Alaska"},
new State(){ID="Arizona",Text="Arizona"},
new State(){ID="Arkansas",Text="Arkansas"},
new State(){ID="California",Text="California"},
new State(){ID="Colorado",Text="Colorado"},
new State(){ID="Connecticut",Text="Connecticut  "},
new State(){ID="Delaware",Text="Delaware  "},
new State(){ID="Florida",Text="Florida  "},
new State(){ID="Georgia",Text="Georgia  "},
new State(){ID="Hawaii",Text="Hawaii  "},
new State(){ID="Idaho",Text="Idaho  "},
new State(){ID="Illinois",Text="Illinois  "},
new State(){ID="Indiana",Text="Indiana  "},
new State(){ID="Iowa",Text="Iowa  "},
new State(){ID="Kansas",Text="Kansas  "},
new State(){ID="Kentucky",Text="Kentucky  "},
new State(){ID="Louisiana",Text="Louisiana  "},
new State(){ID="Maine",Text="Maine  "},
new State(){ID="Maryland",Text="Maryland  "},
new State(){ID="Massachusetts",Text="Massachusetts  "},
new State(){ID="Michigan",Text="Michigan  "},
new State(){ID="Minnesota",Text="Minnesota  "},
new State(){ID="Mississippi",Text="Mississippi  "},
new State(){ID="Missouri",Text="Missouri  "},
new State(){ID="Montana",Text="Montana  "},
new State(){ID="Nebraska",Text="Nebraska  "},
new State(){ID="Nevada",Text="Nevada  "},
new State(){ID="New Hampshire",Text="New Hampshire  "},
new State(){ID="New Jersey",Text="New Jersey  "},
new State(){ID="New Mexico",Text="New Mexico  "},
new State(){ID="New York",Text="New York  "},
new State(){ID="North Carolina",Text="North Carolina  "},
new State(){ID="North Dakota",Text="North Dakota  "},
new State(){ID="Ohio",Text="Ohio  "},
new State(){ID="Oklahoma",Text="Oklahoma  "},
new State(){ID="Oregon",Text="Oregon  "},
new State(){ID="Pennsylvania",Text="Pennsylvania  "},
new State(){ID="Rhode Island",Text="Rhode Island  "},
new State(){ID="South Carolina",Text="South Carolina  "},
new State(){ID="South Dakota  ",Text="South Dakota  "},
new State(){ID="Tennessee  ",Text="Tennessee  "},
new State(){ID="Texas",Text="Texas  "},
new State(){ID="Utah",Text="Utah  "},
new State(){ID="Vermont",Text="Vermont  "},
new State(){ID="Virginia",Text="Virginia  "},
new State(){ID="Washington",Text="Washington  "},
new State(){ID="West Virginia",Text="West Virginia  "},
new State(){ID="Wisconsin",Text="Wisconsin  "},
new State(){ID="Wyoming",Text="Wyoming ;"},
};

            //List<SelectListItem> states = new List<SelectListItem>();

            //states.Add(new SelectListItem { Text = "Select", Value = "Select" });
            //states.Add(new SelectListItem { Text = "Texas", Value = "Texas" });
            //states.Add(new SelectListItem { Text = "Wlington", Value = "Wlington" });
            //states.Add(new SelectListItem { Text = "New Jersy", Value = "New Jersy" });
            //states.Add(new SelectListItem { Text = "Missourie", Value = "Missourie" });
            //states.Add(new SelectListItem { Text = "Ohio", Value = "Ohio" });
            //states.Add(new SelectListItem { Text = "Illonis", Value = "Illonis" });
            //states.Add(new SelectListItem { Text = "Nevada", Value = "Nevada" });
            //states.Add(new SelectListItem { Text = "North Dakota", Value = "North Dakota" });
            //states.Add(new SelectListItem { Text = "Montana", Value = "Montana" });
            return states;
        }


        public static List<CouponType> CouponTypeList()
        {
            List<CouponType> list = new List<CouponType>(){new CouponType(){Code="CPNFIX",Description="CPNFIX-Fixed Amount Off Coupon"}
                                                            ,new CouponType(){Code="CPNPCT",Description="CPNPCT-Percent-Off Coupon"},
                                                             new CouponType(){Code="FIXSHIP",Description="FIXSHIP-Fixed Shipping Coupon"},
                                                             new CouponType(){Code="FREESHIP",Description="FREESHIP-Free Shipping Coupon"},
                                                               new CouponType(){Code="GFTCRT",Description="GFTCRT-Gift Certificate"}};

            return list;
        }


        public static List<CouponUsage> CouponUsageList()
        {
            List<CouponUsage> list = new List<CouponUsage>(){ new CouponUsage(){ ID=1,Description="Unlimited Use Coupon"},
              new CouponUsage(){ ID=2,Description="Single Use per-user Coupon"},
              new CouponUsage(){ ID=3,Description="Single Use per-site Coupon"}};
            return list;
        }


        public static List<ProductType> ProductTypes()
        {
            List<ProductType> list = new List<ProductType>(){ new ProductType(){ ID=1,Description="Product"},
                //new ProductType(){ ID=2,Description="Subscription"},
              new ProductType(){ ID=3,Description="Donation"}};
            return list;
        }


        public static List<Grade> Grades()
        {
            List<Grade> list = new List<Grade>(){ new Grade(){ ID=1,Description="PK"},
              new Grade(){ ID=2,Description="K"},
              new Grade(){ ID=3,Description="1"},
            new Grade(){ ID=4,Description="2"},
            new Grade(){ ID=5,Description="3"},
            new Grade(){ ID=6,Description="4"},
            new Grade(){ ID=7,Description="5"},
            new Grade(){ ID=8,Description="6"},
            new Grade(){ ID=9,Description="7"},
            new Grade(){ ID=10,Description="8"},
            new Grade(){ ID=11,Description="9"},
            new Grade(){ ID=12,Description="10"},
            new Grade(){ ID=12,Description="11"},
            new Grade(){ ID=12,Description="12"}};
            return list;
        }


    }

    public class Country
    {
        public string ID { get; set; }
        public string Text { get; set; } 
    }


    public class State
    {
        public string ID { get; set; }
        public string Text { get; set; } 
    }


    public class Catalog
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }
   


    public class GoalType
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }


    public class ShipAmount
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }

    public class PricingLevel
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }

    public class ParticipantOption
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }
    public class FAQ
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }

    public class CouponType
    {

        public string Code { get; set; }
        public string Description { get; set; }
    }


    public class CouponUsage
    {
        public int ID { get; set; }
        public string Description { get; set; }
        
    }


    public class ProductType
    {
        public int ID { get; set; }
        public string Description { get; set; }     
    }

    public class Grade
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }


    //public class Country
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //}
}