using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
    public class PurchaseOrderStockDetail
    {
        public int PurchaseOrderDetailID { get; set; }
        public int StockItemID { get; set; }
        public string VendorStockNumber { get; set; }
        public string StockItemDescription { get; set; }
        public int QuantityOnOrder { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityOutstanding { get { return QuantityOnOrder - QuantityReceived; }}
    }
}
