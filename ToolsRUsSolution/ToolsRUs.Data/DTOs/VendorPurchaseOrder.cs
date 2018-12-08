using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ToolsRUs.Data.POCOs;
#endregion

namespace ToolsRUs.Data.DTOs
{
    public class VendorPurchaseOrder
    {
        public int PurchaseOrderID { get; set; }
        public int? PurchaseOrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string VendorName { get; set; }
        public string Phone { get; set; }
        public List<PurchaseOrderStockDetail> VendorOrderDetail { get; set; }
    }
}
