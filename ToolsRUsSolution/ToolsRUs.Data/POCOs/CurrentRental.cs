using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
    public class CurrentRental
    {

        public int DetailID { get; set; }
        public int EquipmentID { get; set; }

        public string Description { get; set; }

        public string SerialNumber { get; set; }

        public decimal Rate { get; set; }

        public string ConditionOut { get; set; }

        public double Days { get; set; }

    }

}
