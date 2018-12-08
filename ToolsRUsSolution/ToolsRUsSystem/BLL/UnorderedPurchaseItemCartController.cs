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
    public class UnorderedPurchaseItemCartController
    { 
        public List<UnorderedPurchaseItemCart> List_UnorderedPurchaseItemCart(int purchaseorderid)
        {
            using (var context = new ToolsRUsContext())
            {
                return context.UnorderedPurchaseItemCarts.OrderBy(x => x.Description).Where(x => x.PurchaseOrderID == purchaseorderid).Select(x => x).ToList();
            }
        }

        public void Add_UnorderedPurchaseItemCart(UnorderedPurchaseItemCart item)
        {
            using (var context = new ToolsRUsContext())
            {
                context.UnorderedPurchaseItemCarts.Add(item);
                context.SaveChanges();
            }
        }

        public void Delete_UnorderedPurchaseItemCart(int cartid)
        {
            using (var context = new ToolsRUsContext())
            {
                var exists = context.UnorderedPurchaseItemCarts.Find(cartid);
                if (exists == null)
                {
                    throw new Exception("Cart item does not exist on file");
                }
                context.UnorderedPurchaseItemCarts.Remove(exists);
                context.SaveChanges();
            }
        }
    }
}
