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
    public class CustomerController
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CustomerInfo> List_CustomerByPhone(string phonenumber)
        {
            using (var context = new ToolsRUsContext())
            {
                var results = from x in context.Customers
                              where x.ContactPhone.Contains(phonenumber)
                              select new CustomerInfo
                              {
                                  Id = x.CustomerID,
                                  Name = x.LastName + "," + x.FirstName,
                                  Address = x.Address,
                                  City = x.City
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Customer Customer_Get(int customerid)
        {
            using (var context = new ToolsRUsContext())
            {
                return context.Customers.Find(customerid);
            }

        }
    }
}
