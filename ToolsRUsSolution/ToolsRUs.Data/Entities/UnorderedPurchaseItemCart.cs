using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace ToolsRUs.Data.Entities
{
    [Table("UnorderedPurchaseItemCart")]
    public class UnorderedPurchaseItemCart
    {
        [Key]
        public int CartID { get; set; }
        public int PurchaseOrderID { get; set; }
        [StringLength(100, ErrorMessage = "Description is a maximum of 100 characters")]
        public string Description { get; set; }
        [StringLength(25, ErrorMessage = "Vendor stock number is a maximum of 25 characters")]
        public string VendorStockNumber { get; set; }
        public int Quantity { get; set; }
    }
}
