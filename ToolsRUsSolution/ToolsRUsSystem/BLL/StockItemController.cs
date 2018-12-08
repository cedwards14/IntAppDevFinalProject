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
#endregion

namespace ToolsRUsSystem.BLL
{
    public class StockItemController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<StockItemList> List_ItemsForCartSelection(int categoryID)
        {
            using (var context = new ToolsRUsContext())
            {
                    var results = from x in context.StockItems
                                  where x.CategoryID == categoryID &&  x.QuantityOnHand > 0 && x.Discontinued == false
                                  orderby x.Description
                                  select new StockItemList
                                  {
                                      StockItemID = x.StockItemID,
                                      PurchasePrice = x.SellingPrice,
                                      Description = x.Description,
                                      QuantityOnHand = x.QuantityOnHand
                                  };
                if(categoryID == -1) //-1 will result in all of the items 
                {
                    results = from x in context.StockItems
                              where x.QuantityOnHand > 0 && x.Discontinued == false
                              orderby x.Description
                              select new StockItemList
                              {
                                  StockItemID = x.StockItemID,
                                  PurchasePrice = x.SellingPrice,
                                  Description = x.Description,
                                  QuantityOnHand = x.QuantityOnHand
                              };
                }
                return results.ToList();
                
            }
        }//eom
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public int Select_ItemCount()
        {
            using (var context = new ToolsRUsContext())
            {
                var count = (from x in context.StockItems
                             select x).Count();
                return count;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public StockItem GetQOH_ByStockItemID(int stockItemID)
        {
            using (var context = new ToolsRUsContext())
            {
                StockItem results = context.StockItems.Find(stockItemID);
                return results;
            }
        }

    }
}
