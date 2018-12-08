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
#endregion

namespace ToolsRUsSystem.BLL
{
    [DataObject]
  public class RentalEquipmentController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]

        public List<AvailableRentals> RentalEquipment_Available()
        {
            using (var context = new ToolsRUsContext())
            {
                var results = from x in context.RentalEquipments
                              where x.Available == true
                              select new AvailableRentals
                              {
                                  ID = x.RentalEquipmentID,
                                  Description = x.Description,
                                  Serial = x.SerialNumber,
                                  Rate = x.DailyRate
                              };
                return results.ToList();
            }
        }
    }
}
