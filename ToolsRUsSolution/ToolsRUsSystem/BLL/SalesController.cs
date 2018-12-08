using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ToolsRUs.Data.Entities;
using ToolsRUsSystem.DAL;
using System.ComponentModel;
using ToolsRUs.Data.POCOs;
using DMIT2018Common.UserControls;
#endregion

namespace ToolsRUsSystem.BLL
{
    public class SalesController
    {
        public void AddToCart(int employeeID, int productId, int quantity)
        {
            List<string> reasons = new List<string>();
            using (var context = new ToolsRUsContext())
            {
                ShoppingCart exists = context.ShoppingCarts.Where(x => x.EmployeeID.Equals(employeeID)).Select(x => x).FirstOrDefault();

                ShoppingCartItem newItem = null;
                int itemNumber = 0;

                if(exists == null)
                {
                    exists = new ShoppingCart();
                    exists.EmployeeID = employeeID;
                    exists.CreatedOn = DateTime.Today;
                    exists = context.ShoppingCarts.Add(exists);
                }
                else
                {
                    itemNumber = exists.ShoppingCartItems.Count();


                    newItem = exists.ShoppingCartItems.SingleOrDefault(x => x.StockItemID == productId);
                    if(newItem != null)
                    {
                        reasons.Add("Item already in cart");
                    }
                }
                if(reasons.Count > 0)
                {
                    throw new BusinessRuleException("Adding item to cart", reasons);
                }
                else
                {
                    newItem = new ShoppingCartItem();
                    newItem.StockItemID = productId;
                    newItem.Quantity = quantity;
                    exists.UpdatedOn = DateTime.Today;
                    exists.ShoppingCartItems.Add(newItem);
                    context.SaveChanges();
                }
                
            }
        }
        public int ShoppingCartIdByEmployeeID(int employeeID)
        {
            using (var context = new ToolsRUsContext())
            {
                int shoppingCartID = (from x in context.ShoppingCarts where x.EmployeeID == employeeID select x.ShoppingCartID).FirstOrDefault();
                return shoppingCartID;
            }
         }
        public void RemoveCartAndItems(int employeeID)
        {
            List<string> reasons = new List<string>();
            using (var context = new ToolsRUsContext())
            {
                ShoppingCart exists = context.ShoppingCarts.Where(x => x.EmployeeID.Equals(employeeID)).Select(x => x).FirstOrDefault();

                
                if(exists == null)
                {
                    reasons.Add("Cart Already Deleted");
                }
                else
                {
                    List<ShoppingCartItem> itemList = context.ShoppingCartItems.Where(x => x.ShoppingCartID.Equals(exists.ShoppingCartID)).Select(x => x).ToList();
                    if (reasons.Count > 0)
                    {
                        throw new BusinessRuleException("Cancel", reasons);
                    }
                    else
                    {
                        foreach(ShoppingCartItem item in itemList)
                        {
                            context.ShoppingCartItems.Remove(item);
                        }
                        context.ShoppingCarts.Remove(exists);
                        context.SaveChanges();
                    }
                }
                

            }
        }
        public void PlaceOrder(int employeeID, string paymentType, int? couponID)
        {
            List<string> reasons = new List<string>();
            using (var context = new ToolsRUsContext())
            {
                ShoppingCart cart = context.ShoppingCarts.Where(x => x.EmployeeID.Equals(employeeID)).Select(x => x).FirstOrDefault();
                if(cart == null)
                {
                    reasons.Add("Error finding cart");
                }
                else
                {
                    List<ShoppingCartItem> cartItemsList = context.ShoppingCartItems.Where(x => x.ShoppingCartID.Equals(cart.ShoppingCartID)).Select(x => x).ToList();
                    if(cartItemsList.Count < 1)
                    {
                        reasons.Add("No items in cart");
                    }
                    else
                    {
                        //create sale
                        Sale sale = new Sale();
                        sale.SaleDate = DateTime.Now.Date;
                        sale.PaymentType = paymentType;
                        sale.CouponID = couponID;
                        sale.EmployeeID = employeeID;
                        List<SaleDetail> saleDetailsList = new List<SaleDetail>();

                        decimal subtotal = 0;



                        foreach (ShoppingCartItem item in cartItemsList)
                        {
                            SaleDetail temp = new SaleDetail();
                            StockItem tempStockItem = new StockItem();

                            tempStockItem = context.StockItems.Find(item.StockItemID);

                            temp.SaleID = sale.SaleID;
                            temp.StockItemID = item.StockItemID;
                            temp.SellingPrice = tempStockItem.SellingPrice;
                            temp.Quantity = item.Quantity;
                            saleDetailsList.Add(temp);
                            subtotal += (temp.SellingPrice * temp.Quantity);

                            if (temp.Quantity > tempStockItem.QuantityOnHand)
                            {
                                reasons.Add("Not enough stock for order");
                            }
                            else
                            {
                                tempStockItem.QuantityOnHand = tempStockItem.QuantityOnHand - temp.Quantity;
                                context.Entry(tempStockItem).Property(y => y.QuantityOnHand).IsModified = true;
                            }
                        }
                        sale.SaleDetails = saleDetailsList;
                        sale.SubTotal = Math.Round(subtotal,2);
                        sale.TaxAmount = Math.Round((subtotal * (decimal)0.05),2);
                        if(reasons.Count > 0)
                        {
                            throw new BusinessRuleException("Place Order", reasons);
                        }
                        else
                        {
                            RemoveCartAndItems(employeeID);
                            context.Sales.Add(sale);
                            foreach(SaleDetail item in sale.SaleDetails)
                            {
                                context.SaleDetails.Add(item);
                            }                           
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
