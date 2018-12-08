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
	[DataObject]
	public class VendorStockItemController
	{
		public List<VendorStockItem> List_VendorStockItems(int vendorid)
		{
			using (var context = new ToolsRUsContext())
			{
				var result = (from x in context.StockItems
							  where x.VendorID == vendorid
							  select new VendorStockItem
							  {
								  SID = x.StockItemID,
								  Description = x.Description,
								  QoH = x.QuantityOnHand,
								  QoO = x.QuantityOnOrder,
								  RoL = x.ReOrderLevel,
								  Price = x.PurchasePrice
							  }).ToList();

				return result;
			}		
		}
    }
}
