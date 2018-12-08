using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
    public class RentalReturnDetails
    {
        public int EquipmentId { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public decimal DailyRate { get; set; }
        public string ConditionIn { get; set; }
        public string ConditionOut { get; set; }
        public string Comment { get; set; }
        public double Days { get; set; }

    }
}
