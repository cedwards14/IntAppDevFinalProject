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
    public class VendorController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Vendor> Vendor_List()
        {
            using (var context = new ToolsRUsContext())
            {
                return context.Vendors.OrderBy(x => x.VendorName).ToList();
            }
        }

		public VendorInfo Get_Vendor_Info(int vendorid)
		{
			using (var context = new ToolsRUsContext())
			{
				var result = from x in context.Vendors
							 where x.VendorID == vendorid
							 select new VendorInfo
							 {
								 VendorID = x.VendorID,
								 Address = x.Address,
								 City = x.City,
								 Province = x.Province,
								 PostalCode = x.PostalCode,
								 Phone = x.Phone
							 };

				return result.FirstOrDefault();
			}
		}

    }
}
