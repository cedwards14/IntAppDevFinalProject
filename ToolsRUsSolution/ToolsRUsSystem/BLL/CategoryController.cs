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
    public class CategoryController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CategoryList> List_Categories()
        {
            using (var context = new ToolsRUsContext())
            {
                var results = from x in context.Categories
                              orderby x.Description
                              select new CategoryList
                              {
                                  CategoryID = x.CategoryID,
                                  Description = x.Description,
                                  ItemCount = (from y in x.StockItems where y.QuantityOnHand > 0 select y).Count()
                              };
                return results.ToList();
            }
        }
    }
}
