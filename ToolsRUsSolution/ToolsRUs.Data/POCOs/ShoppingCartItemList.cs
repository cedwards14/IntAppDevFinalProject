using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
    public class ShoppingCartItemList
    {
        public int ShoppingCartItemID { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
