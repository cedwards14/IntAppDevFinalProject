using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsRUs.Data.POCOs;

namespace ToolsRUs.Data.DTOs
{
    public class RentalDetailInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? Coupon { get; set; }
        public string CreditCard { get; set; }
        public DateTime RentalDate { get; set; }
        public List<RentalReturnDetails> RentalItems { get; set; }
    }
}
