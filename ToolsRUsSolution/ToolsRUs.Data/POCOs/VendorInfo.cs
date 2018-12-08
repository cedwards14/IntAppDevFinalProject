using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsRUs.Data.POCOs
{
	public class VendorInfo
	{
		public int VendorID { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string PostalCode { get; set; }
		public string Phone { get; set; }

		public string CityProvince
		{
			get
			{
				return City + ", " + Province;
			}
		}
	}
}
