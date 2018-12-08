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
    public class ShoppingCartItemController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ShoppingCartItemList> List_ItemsFromCart(int ShoppingCartID)
        {
            using (var context = new ToolsRUsContext())
            {
                var results = from x in context.ShoppingCartItems
                              where x.ShoppingCartID == ShoppingCartID
                              select new ShoppingCartItemList
                              {
                                  Description = x.StockItem.Description,
                                  ShoppingCartItemID = x.ShoppingCartItemID,
                                  Quantity = x.Quantity,
                                  PurchasePrice = x.StockItem.SellingPrice

                              };
                return results.ToList();
                              
            }

        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void ShoppingCartItem_Delete(int shoppingCartItemID)
        { 
            using (var context = new ToolsRUsContext())
            {
                var existing = context.ShoppingCartItems.Find(shoppingCartItemID);
                if (existing == null)
                {
                    throw new Exception("Item does not exist on file");
                }
                context.ShoppingCartItems.Remove(existing);

                //Update Shopping carts updated on
                int shoppingCartID = existing.ShoppingCartID;
                ShoppingCart sc = context.ShoppingCarts.Find(shoppingCartID);
                sc.UpdatedOn = DateTime.Now.Date;

                context.SaveChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void ShoppingCartItem_Update(int shoppingCartItemID, int qty)
        {
            using (var context = new ToolsRUsContext())
            {
                ShoppingCartItem item = context.ShoppingCartItems.Find(shoppingCartItemID);
                if(item == null)
                {
                    throw new Exception("Item does not exist on file");
                }
                item.Quantity = qty;

                //Update Shopping carts updated on
                int shoppingCartID = item.ShoppingCartID;
                ShoppingCart sc = context.ShoppingCarts.Find(shoppingCartID);
                sc.UpdatedOn = DateTime.Now.Date;

                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShoppingCartItem GetShoppingCartItem_ByID(int shoppingCartItemID)
        {
            using (var context = new ToolsRUsContext())
            {
                ShoppingCartItem results = context.ShoppingCartItems.Find(shoppingCartItemID);
                return results;
            }
        }
    }
}
