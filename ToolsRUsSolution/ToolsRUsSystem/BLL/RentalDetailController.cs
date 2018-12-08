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
using DMIT2018Common.UserControls;
#endregion

namespace ToolsRUsSystem.BLL
{
    [DataObject]
    public class RentalDetailController
    {


        public void Add_RentalDetailToRental(int employeeid, int rentalequipmentid, int customerid, string credit)
        {
            List<string> reasons = new List<string>();
            using (var context = new ToolsRUsContext())
            {

                //one rental per day

                DateTime date = new DateTime();
                date = DateTime.Today;

                //one rental per hour

                //var dateNow = DateTime.Now;
                //var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, 0, 0);

                //one rental per minute

                //var dateNow = DateTime.Now;
                //var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, 0);


                Rental exists = context.Rentals.Where(x => x.EmployeeID.Equals(employeeid) && x.CustomerID.Equals(customerid) && x.RentalDate.Equals(date)).Select(x => x).FirstOrDefault();
                RentalEquipment equipment = context.RentalEquipments.Where(x => x.RentalEquipmentID.Equals(rentalequipmentid)).Select(x => x).SingleOrDefault();

                RentalDetail newRental = null;

                if (exists == null)
                {
                    exists = new Rental();
                    exists.CustomerID = customerid;
                    exists.EmployeeID = employeeid;
                    exists.RentalDate = date;
                    exists.CreditCard = credit;
                    exists.PaymentType = "C";
                    exists = context.Rentals.Add(exists);
                }
                else
                {

                }
                if (reasons.Count() > 0)
                {
                    throw new BusinessRuleException("something!", reasons);
                }
                else
                {
                    newRental = new RentalDetail();
                    newRental.RentalEquipmentID = rentalequipmentid;
                    newRental.ConditionOut = equipment.Condition;
                    newRental.ConditionIn = "Good";
                    newRental.Days = 1;
                    newRental.DailyRate = equipment.DailyRate;
                    exists.RentalDetails.Add(newRental);
                    equipment.Available = false;
                    context.SaveChanges();
                }
            }
        }//EOM

        public List<CurrentRental> List_EquipmentForRental(int customerid, int employeeid, int equipmentid)
        {
            using (var context = new ToolsRUsContext())
            {
                //one order per day

                DateTime date = new DateTime();
                date = DateTime.Today;

                //one rental per hour

                //var dateNow = DateTime.Now;
                //var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, 0, 0);

                //one rental per minute

                //var dateNow = DateTime.Now;
                //var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, 0);



                var results = from x in context.RentalDetails
                              where x.Rental.RentalDate.Equals(date)
                              && x.Rental.CustomerID.Equals(customerid)
                              && x.Rental.EmployeeID.Equals(employeeid)
                              select new CurrentRental
                              {
                                  DetailID = x.RentalDetailID,
                                  EquipmentID = x.RentalEquipmentID,
                                  Description = x.RentalEquipment.Description,
                                  SerialNumber = x.RentalEquipment.SerialNumber,
                                  Rate = x.DailyRate,
                                  ConditionOut = x.ConditionOut

                              };

                return results.ToList();
            }
        }

        public void Remove_RentalEquipment(int employeeid, int equipmentid, int customerid, int detailid)
        {
            using (var context = new ToolsRUsContext())
            {

                //one rental per day

                DateTime date = new DateTime();
                date = DateTime.Today;

                //one rental per hour

                //var dateNow = DateTime.Now;
                //var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, 0, 0);

                //one rental per minute

                //var dateNow = DateTime.Now;
                //var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, 0);




                var rental = context.Rentals.Where(x => x.EmployeeID.Equals(employeeid) && x.CustomerID.Equals(customerid) && x.RentalDate.Equals(date)).Select(x => x).FirstOrDefault();


                RentalDetail detail = rental.RentalDetails.Where(x => x.RentalDetailID.Equals(detailid) && x.RentalEquipmentID.Equals(equipmentid)).FirstOrDefault();


                if (rental == null)
                {
                    throw new Exception("Rental could not be found!");
                }
                else
                {

                    context.RentalDetails.Remove(detail);
                    RentalEquipment equipment = context.RentalEquipments.Where(x => x.RentalEquipmentID.Equals(equipmentid)).Select(x => x).SingleOrDefault();
                    equipment.Available = true;
                    context.SaveChanges();
                }


            }
        }//EOM

        public void Coupon_Rental(int detailid, int Couponid)
        {
            using (var context = new ToolsRUsContext())
            {
                var exists = (from x in context.RentalDetails
                              where x.RentalDetailID.Equals(detailid)
                              select x).FirstOrDefault();

                if (exists == null)
                {
                    throw new Exception("This Item has been Deleted");

                }
                else
                {
                    exists.Rental.CouponID = Couponid;
                    context.SaveChanges();
                }

            }
        }//EOM


        public void Cancel_Rental(int employeeid, int Customerid, int detailid)
        {
            using (var context = new ToolsRUsContext())
            {
                DateTime date = new DateTime();
                date = DateTime.Today;
                var rental = context.Rentals.Where(x => x.EmployeeID.Equals(employeeid) && x.CustomerID.Equals(Customerid) && x.RentalDate.Equals(date)).Select(x => x).FirstOrDefault();
                if (rental == null)
                {
                    throw new Exception("This order does not exist!");
                }
                else
                {
                    if (rental.SubTotal != 0)
                    {
                        throw new Exception("Cannot cancel an order that has a returned item on it");
                    }
                    else
                    {
                        List<RentalDetail> rentaldetails = (from y in context.RentalDetails
                                                            where y.RentalID == rental.RentalID
                                                            select y).ToList();
                        if (rentaldetails == null)
                        {
                            throw new Exception("There are no items on this rental");
                        }
                        else
                        {
                            for (int rowindex = 0; rowindex < rentaldetails.Count; rowindex++)
                            {
                                int eid = rentaldetails[rowindex].RentalEquipmentID;
                                RentalEquipment equipment = context.RentalEquipments.Where(x => x.RentalEquipmentID.Equals(eid)).Select(x => x).SingleOrDefault();
                                equipment.Available = true;
                                context.RentalDetails.Remove(rentaldetails[rowindex]);
                            }
                            context.Rentals.Remove(rental);
                            context.SaveChanges();
                        }
                    }
                }
            }
        } //EOM

        public void Complete_RentalReturn(Boolean badConditionCheck, int rentalid, List<RentalReturnDetails> returnedRental, double rentaldays)
        {
            using (var context = new ToolsRUsContext())
            {
                var rental = (from x in context.Rentals
                              where x.RentalID == rentalid && x.SubTotal == 0
                              select x).FirstOrDefault();

                if (rental == null)
                {
                    throw new Exception("Order has already been returned");
                }
                else
                {
                    var details = (from y in context.RentalDetails
                                   where y.RentalID == rentalid
                                   select y).ToList();



                    if (badConditionCheck != true)
                    {
                        rental.CreditCard = null;
                    }

                    for (int rowindex = 0; rowindex < returnedRental.Count; rowindex++)
                    {
                        details[rowindex].Days = rentaldays;
                        details[rowindex].Comments = returnedRental[rowindex].Comment == ""? null: returnedRental[rowindex].Comment;
                        details[rowindex].ConditionIn = returnedRental[rowindex].ConditionIn;
                        details[rowindex].RentalEquipment.Condition = returnedRental[rowindex].ConditionIn;
                        //come back to check this again
                        //context.Entry(details[rowindex]).Property(y => y.ConditionIn).IsModified = true;
                        //context.Entry(details[rowindex]).Property(y => y.Comments).IsModified = true;
                        details[rowindex].RentalEquipment.Available = true;
                    }
                    context.SaveChanges();
                }
            }
        }// end of method

        public Rental Accept_Payment(int rentalid, char paymenttype)
        {
            using (var context = new ToolsRUsContext())
            {
                Rental finalinfo = new Rental();

                var rental = (from x in context.Rentals
                              where x.RentalID == rentalid && x.SubTotal == 0
                              select x).FirstOrDefault();

                if (rental == null)
                {
                    throw new Exception("Order has already been returned");
                }
                var details = (from y in context.RentalDetails
                               where y.RentalID == rentalid
                               select y).ToList();

                decimal subtl = 0;
                double CouponDiscount = rental.CouponID == null ? 0.00: ((double)rental.Coupon.CouponDiscount / 100);



                for (int rowindex = 0; rowindex < details.Count; rowindex++)
                {
                    subtl += ((decimal)details[rowindex].DailyRate ) * (decimal)details[rowindex].Days;
                }
                rental.PaymentType = paymenttype.ToString();
                rental.SubTotal = Math.Round(subtl , 2);
                rental.TaxAmount = Math.Round(subtl * (decimal)0.05, 2);
                context.SaveChanges();

                return rental;
            }
        }//EOM

        public List<CurrentRental> mecheating(int rid)
        {
            using (var context = new ToolsRUsContext())
            {
                var detail = context.RentalDetails.Where(x => x.RentalDetailID.Equals(rid)).Select(x => x).FirstOrDefault();

                var rentalorder = from y in context.RentalDetails
                                  where y.RentalID == detail.RentalID
                                  select new CurrentRental
                                  {

                                      DetailID = y.RentalDetailID,
                                      EquipmentID = y.RentalEquipmentID,

                                      Description = y.RentalEquipment.Description,

                                      SerialNumber = y.RentalEquipment.SerialNumber,

                                      Rate = y.DailyRate,

                                      ConditionOut = y.ConditionOut,

                                      Days = y.Days


                                  };

                return rentalorder.ToList();

            }




        }



    }
}
