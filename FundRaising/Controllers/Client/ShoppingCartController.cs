using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FundRaising.Models;
using FundRaising.ViewModels;
using Newtonsoft.Json;

namespace FundRaising.Controllers.Client
{
    public class ShoppingCartController : Controller
    {
        FundRaisingDBContext db = new FundRaisingDBContext();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            if (Session["BrochureID"] != null)
            {
                ViewBag.BrochureID = Session["BrochureID"].ToString();
            }
            else
            {
                return RedirectPermanent("/");
            }

            if (Session["studentID"] != null)
            {
                ViewBag.studentID = Session["studentID"].ToString();
            }
            if (Session["categoryID"] != null && Session["CategoryName"] != null)
            {
                ViewBag.caterogyID = Session["CategoryID"] + "-" + Session["CategoryName"];
            }
            //string cartID=Request.Cookies["CartID"].ToString();
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                cartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                Count=cart.GetCount(),
                IsSalesTaxChargable=cart.IsSalesTaxChargable()
                
            };

            

            double total =cart.GetTotal();
            double shippingamt = 0;
            var shipping = db.ShippingCharges.ToList();
            double freeShippingAmount=0;
            foreach (ShippingCharge charge in shipping)
            {
                if (total >= charge.LowerLimit && total <= charge.UpperLimit)
                {
                    shippingamt = charge.Charge;
                    freeShippingAmount=charge.FreeAmount;
                    break;
                }
            }
            if(Session["Organization"]!=null)
            {
                var org=Session["Organization"] as Organization;
                if(org.FreeShippingAmount)
                {
                    if(total>=freeShippingAmount)
                    {
                        shippingamt = 0;
                    }
                }
            }
            var SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
            ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
            ViewBag.State = SalesTaxCharge.State.ToString();
            if(viewModel.IsSalesTaxChargable)
            {
                //SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                ViewBag.State = SalesTaxCharge.State.ToString();
                ViewBag.IsTaxChargeable = "true";
                
            }
            else
            {
                ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                ViewBag.State = SalesTaxCharge.State.ToString();
                ViewBag.IsTaxChargeable = "false";
               // ViewBag.SalesTaxCharge = "N/A";
            }
            ViewBag.CartView = true;
            ViewBag.shipToSchool = cart.IsShipToSchool();
            ViewBag.ShippingAmount = shippingamt;
            ViewBag.FreeShippingAmount = freeShippingAmount;
            //ViewBag.SalesTax=
            //if (Session["Organization"] != null)
            //{
            //    //Organization org = Session["Organization"] as Organization;
            //    //if (org.FreeShippingAmount>0)
            //    //{
                    
            //    //}
            //    //else
            //    //{
            //    //    ViewBag.ShippingAmount = 0;
            //    //}
            //    double total = viewModel.CartTotal;
            //    double shippingamt = 0;
            //    var shipping = db.ShippingCharges.ToList();
            //    foreach (ShippingCharge charge in shipping)
            //    {
            //        if (total >= charge.LowerLimit && total <= charge.UpperLimit)
            //        {
            //            shippingamt = charge.Charge;
            //            break;
            //        }
            //    }
            //    //double.TryParse(org.FreeShippingAmount.ToString(), out shippingamt);
            //    ViewBag.ShippingAmount = shippingamt; 
                
            //}
            return View(viewModel);
        }

        
        public ActionResult AddtoCart(int ID,int Quantity, string GiftCard="",string IssueValue="",int Option=0)
        {
            GiftCard giftCard = null;
         var product = db.Products.Find(ID);       
         var cart = ShoppingCart.GetCart(this.HttpContext);
         var cartItems = cart.GetCartItems();
         ShoppingCart shop = new ShoppingCart();
            if((product.Inventory && product.InventoryAmount>0) || product.InventoryAmount==-1)
            {             
                if(shop.CheckShipToSchool(cart))
                {
                    ViewBag.shipToSchool = true;
                }
                else
                {
                    ViewBag.shipToSchool = false;
                }
                //if(cartItems.Count>0)
                //{
                //    if (shop.CheckShipToSchool(cart))
                //    {
                //        if (!product.ShipToSchoolOnly)
                //        {
                //            return Json("ShipToSchool", JsonRequestBehavior.AllowGet);
                //        }
                //    }
                //    else
                //    {
                //        if (product.ShipToSchoolOnly)
                //        {
                //            return Json("ShipToSchool1", JsonRequestBehavior.AllowGet);
                //        }
                //    }

                //}
                if(!string.IsNullOrEmpty(GiftCard))
                {
                    giftCard = JsonConvert.DeserializeObject<GiftCard>(GiftCard);
                }
               
                cart.AddToCart(product, Quantity, Option, IssueValue,product.ChargeShipping,product.ChargeSalesTax, giftCard);
                
                //adding View Model to show data in View
                ShoppingCartViewModel shoppingcart = new ShoppingCartViewModel()
                {
                    cartItems = cart.GetCartItems(),
                    Count = cart.GetCount(),
                    CartTotal = cart.GetTotal(),
                    IsSalesTaxChargable=cart.IsSalesTaxChargable()
                };

                if (product.Inventory)
                {
                    //product.InventoryAmount -= Quantity;
                    //db.Entry(product).State = EntityState.Modified;
                    //db.SaveChanges();
                }

                if (Option == 1)
                {
                    //if (Session["Organization"] != null)
                    //{
                    //    Organization org = Session["Organization"] as Organization;
                    //    if (org.FreeShippingAmount > 0)
                    //    {
                    //        double total = shoppingcart.CartTotal;
                    //        double shippingamt = 0;
                    //        double.TryParse(org.FreeShippingAmount.ToString(), out shippingamt);
                    //        ViewBag.FreeShippingAmount = shippingamt - total;
                    //    }
                    //    else
                    //    {
                    //        ViewBag.FreeShippingAmount = 0;
                    //    }

                    //}
                    double total = shoppingcart.CartTotal;
                    double shippingamt = 0, freeShippingAmount=0;
                    var shipping = db.ShippingCharges.ToList();
                    foreach (ShippingCharge charge in shipping)
                    {
                        if (total >= charge.LowerLimit && total <= charge.UpperLimit)
                        {
                            shippingamt = charge.Charge;
                            freeShippingAmount = charge.FreeAmount;
                            break;
                        }
                    }
                    if (Session["Organization"] != null)
                    {
                        var org = Session["Organization"] as Organization;
                        if (org.FreeShippingAmount)
                        {
                            if (total >= freeShippingAmount)
                            {
                                shippingamt = 0;
                            }
                        }
                    }
                    var SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                    ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                    ViewBag.State = SalesTaxCharge.State.ToString();
                    if (shoppingcart.IsSalesTaxChargable)
                    {
                        //SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                        ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                        ViewBag.State = SalesTaxCharge.State.ToString();
                        ViewBag.IsTaxChargeable = "true";

                    }
                    else
                    {
                        ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                        ViewBag.State = SalesTaxCharge.State.ToString();
                        ViewBag.IsTaxChargeable = "false";
                        // ViewBag.SalesTaxCharge = "N/A";
                    }
                    ViewBag.ShippingAmount = shippingamt;
                    ViewBag.FreeShippingAmount = freeShippingAmount;
                    ViewBag.CartView = true;
                    return PartialView("_CartView", shoppingcart);
                }
                else
                {
                    ViewBag.CartView = false;
                    ViewBag.shipToSchool = cart.IsShipToSchool();
                    return Json(shoppingcart, JsonRequestBehavior.AllowGet);
                }

                                                                    
            }

            return Json("OOS", JsonRequestBehavior.AllowGet); 

         
            
        }


        
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the album to display confirmation
            string itemName = db.Carts
            .Single(item => item.ID== id).Description;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message

            ShoppingCartViewModel shoppingcart = new ShoppingCartViewModel()
            {
                cartItems = cart.GetCartItems(),
                Count = cart.GetCount(),
                CartTotal = cart.GetTotal()

            };
            if (Session["BrochureID"] != null)
            {
                ViewBag.BrochureID = Session["BrochureID"].ToString();
            }
            else
            {
                return RedirectPermanent("/");
            }

            if (Session["studentID"] != null)
            {
                ViewBag.studentID = Session["studentID"].ToString();
            }
            if (Session["categoryID"] != null && Session["CategoryName"] != null)
            {
                ViewBag.caterogyID = Session["CategoryID"] + "-" + Session["CategoryName"];
            }
            //var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                cartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                Count = cart.GetCount(),
                IsSalesTaxChargable = cart.IsSalesTaxChargable()
            };

            double total = cart.GetTotal();
            double shippingamt = 0;
            var shipping = db.ShippingCharges.ToList();
            double freeShippingAmount = 0;
            foreach (ShippingCharge charge in shipping)
            {
                if (total >= charge.LowerLimit && total <= charge.UpperLimit)
                {
                    shippingamt = charge.Charge;
                    freeShippingAmount = charge.FreeAmount;
                    break;
                }
            }
            if (Session["Organization"] != null)
            {
                var org = Session["Organization"] as Organization;
                if (org.FreeShippingAmount)
                {
                    if (total >= freeShippingAmount)
                    {
                        shippingamt = 0;
                    }
                }
            }
            var SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
            ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
            ViewBag.State = SalesTaxCharge.State.ToString();
            if (viewModel.IsSalesTaxChargable)
            {
                //SalesTaxCharge = db.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
                ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                ViewBag.State = SalesTaxCharge.State.ToString();
                ViewBag.IsTaxChargeable = "true";

            }
            else
            {
                ViewBag.SalesTaxCharge = SalesTaxCharge.TaxAmount.ToString();
                ViewBag.State = SalesTaxCharge.State.ToString();
                ViewBag.IsTaxChargeable = "false";
                // ViewBag.SalesTaxCharge = "N/A";
            }
            ViewBag.shipToSchool = cart.IsShipToSchool();
            ViewBag.ShippingAmount = shippingamt;
            ViewBag.FreeShippingAmount = freeShippingAmount;
            ViewBag.CartView = true;
            //var result = "The item" + itemName + "has been removed from your cart.";
            return PartialView("_CartView",shoppingcart);
        }


        public ActionResult EmptyCart()
        {

            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart();
            ShoppingCartViewModel shoppingcart = new ShoppingCartViewModel()
            {
                cartItems = cart.GetCartItems(),
                Count = cart.GetCount(),
                CartTotal = cart.GetTotal()

            };
            ViewBag.shipToSchool = false;   
            //int count = cart.GetCount();
            return PartialView("_CartView", shoppingcart);
           // return Json("Delete",JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.Count= cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
