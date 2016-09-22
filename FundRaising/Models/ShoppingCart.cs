using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundRaising.Models
{
    public partial class ShoppingCart
    {
        FundRaisingDBContext storeDb = new FundRaisingDBContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public  bool CheckShipToSchool(ShoppingCart cart)
        {
            List<Cart> cartItems = new List<Cart>();
         Product cartProduct =null;
            int productID=0;        
         cartItems = cart.GetCartItems();
         bool result = false;
         if (cartItems.Count > 0)
         {
             foreach(Cart c in cartItems)
             {
                 productID = c.productId;
                 cartProduct = storeDb.Products.Find(productID);
                 if (cartProduct.ShipToSchoolOnly)
                 {
                     result=true;
                     break;
                 }
                 else
                 {
                     result=false;
                 }  
             }
             return result; 
         }
         return false;   
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }


        public void AddToCart(Product product, int Quantity, int Option,string  IssueValue,bool chargeshipping,bool chargeSalesTax,GiftCard giftCard)
        {
            Cart CartItem=null;
            string [] splitData=null;
            int issue = 0,IssuePrice=0;
            bool IsGiftcard = false;
            if (!string.IsNullOrEmpty(IssueValue) && IssueValue!="0")
            {
                splitData = IssueValue.Split('-');
                int.TryParse(splitData[0], out IssuePrice);
                int.TryParse(splitData[1], out issue);
            }
            if(giftCard!=null)
            {
                IsGiftcard = true;
            }
            if(IssuePrice>0)
            {
                
                    CartItem = storeDb.Carts.SingleOrDefault(c => c.CartID == ShoppingCartId && c.itemNumber == product.ItemNumber && c.Price == IssuePrice && c.IsGift==IsGiftcard);                                
            }
            else
            {
                try
                {
                    CartItem = storeDb.Carts.SingleOrDefault(c => c.CartID == ShoppingCartId && c.itemNumber == product.ItemNumber);
                }
                catch(Exception ex)
                {

                }
                 
            }
            
            if (CartItem == null)
            {
                if(IssuePrice>0)
                {                                        
                    CartItem = new Cart { itemNumber = product.ItemNumber, productId = product.ID, Description = product.Description +" Issues for:"+ issue.ToString(), CartID = ShoppingCartId, Quantity = Quantity, Price = IssuePrice, DateCreated = DateTime.Now ,chargeShipping=chargeshipping,ShipToSchool=product.ShipToSchoolOnly,IsGift=IsGiftcard};
                    
                }
                else
                {
                    CartItem = new Cart { itemNumber = product.ItemNumber, productId = product.ID, Description = product.Description, CartID = ShoppingCartId, Quantity = Quantity, Price = product.CustomerRetailPrice, DateCreated = DateTime.Now, chargeShipping = chargeshipping, chargeSalesTax = chargeSalesTax, ShipToSchool = product.ShipToSchoolOnly };
                }
                
                storeDb.Carts.Add(CartItem);                
            }
            else
            {
                if (Option == 1)
                {

                    CartItem.Quantity = Quantity;
                    //CartItem.Price = product.CustomerRetailPrice;
                }
                else
                {
                    CartItem.Quantity += Quantity;
                }
                //if (IssuePrice > 0)
                //{
                //    CartItem = new Cart { itemNumber = product.ItemNumber, productId = product.ID, Description = product.Description +" Issues for:"+issue.ToString(), CartID = ShoppingCartId, Quantity = Quantity, Price = IssuePrice, DateCreated = DateTime.Now };
                //    storeDb.Carts.Add(CartItem);
                //}
                //else
                //{
                   
                //}
                
            }
            double cartTotal = GetTotal();
            if(cartTotal>=75)
            {
                List<Cart> cartItems = GetCartItems();
                cartItems.ForEach(x => { x.chargeShipping = false; });
            }
            storeDb.SaveChanges();
            if (IsGiftcard)
            {
                giftCard.CartITemID = CartItem.ID;
                storeDb.GiftCards.Add(giftCard);
                storeDb.SaveChanges();
            }
        }

        public int RemoveFromCart(int Id)
        {
            var cartItem = storeDb.Carts.Single(cart =>cart.CartID == ShoppingCartId && cart.ID== Id);

            int itemCount = 0;
            if (cartItem != null)
            {
                //if (cartItem.Quantity > 0)
                //{
                //    cartItem.Quantity--;
                //    itemCount = cartItem.Quantity;
                //}
                //else
                //{
                //    storeDb.Carts.Remove(cartItem);
                //}
                storeDb.Carts.Remove(cartItem);
                storeDb.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = storeDb.Carts.Where(cart => cart.CartID == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                storeDb.Carts.Remove(cartItem);
            }

            storeDb.SaveChanges();
        }


        public List<Cart> GetCartItems()
        {
                return storeDb.Database.SqlQuery<Cart>("exec sp_GetCartItems @CartID", new SqlParameter("@cartID", ShoppingCartId)).ToList();            
        }




        public int GetCount()
        {
            int? count = (from cartItems in storeDb.Carts where cartItems.CartID == ShoppingCartId select (int?)cartItems.Quantity).Sum();

            return count ?? 0;
        }


        public double GetTotal()
        {
            double? total = (from cartItems in storeDb.Carts where cartItems.CartID == ShoppingCartId select (int?)cartItems.Quantity * cartItems.Price).Sum();
            //double? total = (from cartItems in storeDb.Carts where cartItems.CartID == ShoppingCartId select (int?)(cartItems.Price * cartItems.Quantity)).Sum();//

            return total ?? double.MinValue;
        }
      


        public string GetCartId(HttpContextBase context)
       {
            string studentID = "";

            if (context.Session[CartSessionKey] == null)
            {                
                //if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                //{
                //    context.Session[CartSessionKey] = context.User.Identity.Name;
                //}
                //else
                //{                                       
                //}
                if (context.Session["StudentID"] != null)
                {
                    studentID = context.Session["StudentID"].ToString();
                }

                Guid tempCartId = Guid.NewGuid();
                context.Session[CartSessionKey] = tempCartId.ToString() + "|" + studentID;

                HttpCookie cook = new HttpCookie(studentID);
                cook.Value = context.Session[CartSessionKey].ToString();
                cook.Expires = DateTime.Now.AddDays(10);
                context.Response.Cookies.Add(cook);

            }
            else
            {
                string cartKey = context.Session[CartSessionKey].ToString();
                var cartKeys = cartKey.Split('|');
                
                if (context.Session["StudentID"] != null)
                {
                    studentID = context.Session["StudentID"].ToString();
                }
                if (!cartKeys[1].Equals(studentID,StringComparison.InvariantCultureIgnoreCase))
                {
                    string cartCookie = "";
                    if (context.Request.Cookies[studentID] != null)
                    {
                        cartCookie = context.Request.Cookies[studentID].Value.ToString();
                        var cart = storeDb.Carts.Where(x => x.CartID == cartCookie).SingleOrDefault();
                        if(cart!=null)
                        {
                            context.Session[CartSessionKey] = cart.CartID.ToString();
                        }
                        
                    }
                    else
                    {
                        Guid tempCartId = Guid.NewGuid();
                        context.Session[CartSessionKey] = tempCartId.ToString() + "|" + studentID;                    
                    }
                    //return context.Session[CartSessionKey].ToString();
                }
                HttpCookie cook = new HttpCookie(studentID);
                cook.Value = context.Session[CartSessionKey].ToString();
                cook.Expires = DateTime.Now.AddDays(10);
                context.Response.Cookies.Add(cook);
            }
           return context.Session[CartSessionKey].ToString();
        }
            
        public void MigrateCart(string username)
        {
            var shoppingCart = storeDb.Carts.Where(c => c.CartID == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartID = username;
            }
            storeDb.SaveChanges();
        }


        public int createOrder(Order order)
        {
            double orderTotal = 0;

            var CartItems = GetCartItems();
            Product p=null;
            Organization org = null;
            OrderDetail orderDetail = new Models.OrderDetail() ;
            double cartTotal = 0;
            bool ISChargeSalesTax=false ;
            int totalQuantity = 0;
            double ShippingAmount = 0, SalesTaxAmount = 0;
            bool ShipToSchool = false;
            
            var salesTax = storeDb.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
            if(salesTax!=null)
            {
                if(ShrdMaster.Instance.CheckState(order.SState,salesTax.State))
                {
                    ISChargeSalesTax = true;
                    SalesTaxAmount = salesTax.TaxAmount;
                }
            }

            foreach (var item in CartItems)
            {
                p = storeDb.Products.Find(item.productId);
                if(item.ShipToSchool)
                {
                    ShipToSchool=true;
                }
                if(p.InventoryAmount>0 ||p.InventoryAmount==-1)
                {
                    orderDetail = new OrderDetail
                    {
                        itemNumber= item.itemNumber,
                        ProductID=item.productId,
                        OrderID = order.ID,
                        Quantity = item.Quantity,
                        unitPrice = item.Price,
                        ChargeShipping=item.chargeShipping,
                        ChargeSalesTax=item.chargeSalesTax
                    };
                    if(item.chargeShipping)
                    {
                        if (!ShipToSchool)
                        {
                            ShippingCharge charge = storeDb.ShippingCharges.Where(x => cartTotal >= x.LowerLimit && cartTotal <= x.UpperLimit).SingleOrDefault();
                            if (charge != null)
                            {
                                if (org != null)
                                {
                                    if (org.FreeShippingAmount)
                                    {
                                        ShippingAmount = 0;
                                    }
                                    else
                                    {
                                        ShippingAmount += charge.Charge;
                                    }

                                }
                                else
                                {
                                    ShippingAmount += charge.Charge;
                                }


                            }
                        }

                    }
                    orderTotal += (item.Quantity * item.Price);
                    totalQuantity += item.Quantity;
                    storeDb.OrderDetails.Add(orderDetail);
                    if(p.Inventory)
                    {
                        p.InventoryAmount = p.InventoryAmount - item.Quantity;
                    }                   
                    storeDb.Entry(p).State = EntityState.Modified;
                }
                else
                {
                    return -1;
                }
                
            }
            
          //  order.Total = orderTotal;

            //to save the order Summary           
            cartTotal= GetTotal();
           // ISChargeSalesTax = IsSalesTaxChargable();
            org=storeDb.Organizations.Find(order.SchoolID);
            //if(!ShipToSchool)
            //{
            //    ShippingCharge charge = storeDb.ShippingCharges.Where(x => cartTotal >= x.LowerLimit && cartTotal <= x.UpperLimit).SingleOrDefault();
            //    if (charge != null)
            //    {
            //        if (org != null)
            //        {
            //            if (org.FreeShippingAmount)
            //            {
            //                ShippingAmount = 0;
            //            }
            //            else
            //            {
            //                ShippingAmount = charge.Charge;
            //            }

            //        }
            //        else
            //        {
            //            ShippingAmount = charge.Charge;
            //        }


            //    }
            //}
            

            //SalesTaxCharge scharge = storeDb.SalesTaxCharges.Where(x => x.Active == true).SingleOrDefault();
            //if (ISChargeSalesTax)
            //{
            //    SalesTaxAmount = scharge.TaxAmount;
            //}
            OrderSummary ordersum = new OrderSummary();
            ordersum.OrderID = order.ID;
            ordersum.ShippingAmount = ShippingAmount;
            ordersum.TotalAmount = cartTotal;
            ordersum.SalesTax = SalesTaxAmount;
            ordersum.TotalPayable = cartTotal + ShippingAmount + SalesTaxAmount;
            ordersum.Quantity = totalQuantity;
            storeDb.OrderSummaries.Add(ordersum);
            EmptyCart();
            storeDb.SaveChanges();

            //HttpContext.Current.Session["OrderSummary"] = ordersum; 
           // EmptyCart();
            return order.ID;
        }


        public bool IsSalesTaxChargable()
        {
            bool result=false;
            var CartItems = GetCartItems();
            foreach(Cart c in CartItems)
            {
                if(c.chargeSalesTax==true)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool IsShipToSchool()
        {
            bool result = false;
            var CartItems = GetCartItems();
            foreach (Cart c in CartItems)
            {
                if (c.ShipToSchool== true)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        public static ShoppingCart GetCart(string CartID)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = CartID;
            return cart;
        }
    }
}