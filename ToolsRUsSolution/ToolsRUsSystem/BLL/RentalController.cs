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
using ToolsRUs.Data.DTOs;
#endregion

namespace ToolsRUsSystem.BLL
{
    [DataObject]
    public class RentalController
    {

        public List<RentalDetailInfo> Rental_RentalDetails(int rentalid)
        {
            using (var context = new ToolsRUsContext())
            {
                var results = from x in context.Rentals
                              where x.RentalID == rentalid && x.SubTotal == 0
                              select new RentalDetailInfo
                              {
                                  Name = x.Customer.LastName + "," + x.Customer.FirstName,
                                  Address = x.Customer.Address,
                                  City = x.Customer.City,
                                  Coupon = x.CouponID,
                                  CreditCard = x.CreditCard,
                                  RentalDate = x.RentalDate,
                                  RentalItems = (from y in x.RentalDetails
                                                 where y.RentalID == rentalid
                                                 select new RentalReturnDetails
                                                 {
                                                     EquipmentId = y.RentalEquipment.RentalEquipmentID,
                                                     Description = y.RentalEquipment.Description,
                                                     SerialNumber = y.RentalEquipment.SerialNumber,
                                                     DailyRate = y.RentalEquipment.DailyRate,
                                                     ConditionIn = y.ConditionIn,
                                                     ConditionOut = y.ConditionOut,
                                                     Comment = y.Comments,
                                                     Days = y.Days
                                                 }).ToList()
                              };
                return results.ToList();
            }
        }

        public List<CustomerReturn> Customer_ReturnInfo(string phonenumber)
        {
            using (var context = new ToolsRUsContext())
            {
                var results = from x in context.Rentals
                              where x.Customer.ContactPhone.Contains(phonenumber) && x.SubTotal == 0
                              select new CustomerReturn
                              {
                                  rentalid = x.RentalID,
                                  Name = x.Customer.LastName + "," + x.Customer.FirstName,
                                  Address = x.Customer.Address,
                                  RentalDate = x.RentalDate
                              };
                return results.ToList();
            }
        }


     

    }
}
