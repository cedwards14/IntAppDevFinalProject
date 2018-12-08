using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
	public class PurchasingOrder
	{
		public int PurchaseOrderID { get; set; }
		public decimal Subtotal { get; set; }
		public decimal TaxAmount { get; set; }
	}
}
