using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
    public class VendorStockItem
    {
		public int SID { get; set; }
		public string Description { get; set; }
		public int QoH { get; set; }
		public int QoO { get; set; }
		public int RoL { get; set; }
		public int Buffer
		{
			get
			{
				return QoH + QoO - RoL ;
			}
		}
		public decimal Price { get; set; }
    }
}
