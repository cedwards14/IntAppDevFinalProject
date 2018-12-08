using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ToolsRUs.Data.DTOs;
using ToolsRUs.Data.Entities;
using ToolsRUs.Data.POCOs;
using ToolsRUsSystem.BLL;

namespace ToolsRUsWebsite.Rentals
{
    public partial class Returns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.Rentals))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }


            }
            string username = User.Identity.Name;
            ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(username);
            EmployeeName.Text = info.FullName;

        }


        #region Get EmployeeID
        public int ReturnEmployeeID(string username)
        {
            ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(username);
            int employeeid = info.EmployeeID;

            return employeeid;
        }
        #endregion

        protected void RentalSearchLB_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(RentalInfo.Text))
            {
                MessageUserControl.ShowInfo("Please enter a rental ID");
            }
            else
            {

                int rid;
                if (!int.TryParse(RentalInfo.Text, out rid))
                {
                    MessageUserControl.ShowInfo("not a rentalid!");
                }
                else
                {


                    RentalController sysmgr = new RentalController();
                    List<RentalDetailInfo> theRental = sysmgr.Rental_RentalDetails(rid);
                    if (theRental.Count == 0)
                    {
                        MessageUserControl.ShowInfo("This order has already been returned");
                    }
                    else
                    {
                        Name.Text = theRental.SingleOrDefault().Name;
                        Address.Text = theRental.SingleOrDefault().Address;
                        City.Text = theRental.SingleOrDefault().City;
                        creditcardTB.Text = theRental.SingleOrDefault().CreditCard;
                        Date.Text = theRental.SingleOrDefault().RentalDate.ToString();
                        List<RentalReturnDetails> details = theRental.FirstOrDefault().RentalItems;
                        RentalDays.Text = details.FirstOrDefault().Days.ToString();
                        ReturnGV.DataSource = details;
                        ReturnGV.DataBind();
                    }
                }
            }
        }

        protected void PhoneSearchLB_Click(object sender, EventArgs e)
        {
            //if(string.IsNullOrEmpty(RentalInfo.Text))
            //{
            //    MessageUserControl.ShowInfo("Please enter a phone number");
            //}
            //else
            //{
            string phonenumber = AreaCode.Text + "." + Phone1.Text + "." + Phone2.Text;
            //string phonenumber = RentalInfo.Text;
            RentalController sysmgr = new RentalController();
            List<CustomerReturn> customer = sysmgr.Customer_ReturnInfo(phonenumber);
            if (customer.Count == 0)
            {
                MessageUserControl.ShowInfo("No open orders on file for this customer");
            }
            else
            {
                customerGV.DataSource = customer;
                customerGV.DataBind();
            }
            //}
        }

        protected void PullRental_Command(object sender, CommandEventArgs e)
        {
            int rentalid = int.Parse(e.CommandArgument.ToString());

            RentalController sysmgr = new RentalController();
            List<RentalDetailInfo> theRental = sysmgr.Rental_RentalDetails(rentalid);
            if (theRental.Count == 0)
            {
                MessageUserControl.ShowInfo("The Rental has already been returned");
            }
            else
            {
                DateTime date = theRental.SingleOrDefault().RentalDate;
                Name.Text = theRental.SingleOrDefault().Name;
                Address.Text = theRental.SingleOrDefault().Address;
                City.Text = theRental.SingleOrDefault().City;
                creditcardTB.Text = theRental.SingleOrDefault().CreditCard;
                Date.Text = string.Format("{0: MMM dd yyyy}", date);
                List<RentalReturnDetails> details = theRental.FirstOrDefault().RentalItems;
                if (details.Count == 0)
                {
                    MessageUserControl.ShowInfo("There is no equipment on this rental");

                }
                else
                {
                    RentalDays.Text = details.FirstOrDefault().Days.ToString();
                }

                ReturnGV.DataSource = details;
                ReturnGV.DataBind();
            }
        }

        protected void Return_Click(object sender, EventArgs e)
        {
            if (ReturnGV.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("No rental has been selected");

            }
            else
            {
                double rentalDays;
                if (!double.TryParse(RentalDays.Text, out rentalDays))
                {
                    MessageUserControl.ShowInfo("not a rental number ");
                }
                else
                {


                    if (rentalDays < 0.5)
                    {
                        MessageUserControl.ShowInfo("Minimum rental is half a day");
                    }
                    else
                    {
                        int rentalid = 0;
                        if (customerGV.Rows.Count > 0)
                        {
                            rentalid = int.Parse((customerGV.Rows[0].FindControl("rentalid") as Label).Text);
                            //int rentalDetailId = int.Parse((RentalGV.Rows[0].FindControl("DetailID") as Label).Text);
                        }
                        else
                        {
                            rentalid = int.Parse(RentalInfo.Text);
                        }


                        Boolean badConditionCheck = false;
                        List<RentalReturnDetails> returnedRental = new List<RentalReturnDetails>();
                        for (int rowindex = 0; rowindex < ReturnGV.Rows.Count; rowindex++)
                        {
                            RentalReturnDetails returnedItem = new RentalReturnDetails();
                            returnedItem.EquipmentId = int.Parse((ReturnGV.Rows[rowindex].FindControl("EquipmentID") as Label).Text);
                            returnedItem.Description = (ReturnGV.Rows[rowindex].FindControl("Description") as Label).Text;
                            returnedItem.SerialNumber = (ReturnGV.Rows[rowindex].FindControl("SerialNumber") as Label).Text;
                            returnedItem.DailyRate = decimal.Parse((ReturnGV.Rows[rowindex].FindControl("DailyRate") as Label).Text);
                            returnedItem.ConditionOut = (ReturnGV.Rows[rowindex].FindControl("ConditionOut") as Label).Text;
                            returnedItem.ConditionIn = (ReturnGV.Rows[rowindex].FindControl("ConditionIn") as TextBox).Text;
                            returnedItem.Comment = (ReturnGV.Rows[rowindex].FindControl("Comment") as TextBox).Text;
                            if (returnedItem.ConditionIn != "Good")
                            {
                                badConditionCheck = true;
                            }
                            returnedRental.Add(returnedItem);
                        }
                        MessageUserControl.TryRun(() =>
                        {
                            RentalDetailController sysmgr = new RentalDetailController();
                            sysmgr.Complete_RentalReturn(badConditionCheck, rentalid, returnedRental, rentalDays);
                            Pay.Enabled = true;
                        }, "Transaction Complete", "Your rental has been returned!");
                    }
                }
            }
        }

        protected void Pay_Click(object sender, EventArgs e)
        {
            if (ReturnGV.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("Please look up a rental");
            }
            else
            {
                if (paymentMethod.SelectedValue != "C" && paymentMethod.SelectedValue != "D" && paymentMethod.SelectedValue != "M")
                {
                    MessageUserControl.ShowInfo("Please choose a payment Type");
                }
                else
                {

                    char payment = char.Parse(paymentMethod.SelectedValue);
                    int rentalid = 0;
                    if (customerGV.Rows.Count > 0)
                    {
                        rentalid = int.Parse((customerGV.Rows[0].FindControl("rentalid") as Label).Text);
                    }
                    else
                    {
                        rentalid = int.Parse(RentalInfo.Text);
                    }

                    MessageUserControl.TryRun(() =>
                    {
                        RentalDetailController sysmgr = new RentalDetailController();
                        Rental info = sysmgr.Accept_Payment(rentalid, payment);
                        double discountcheck = info.Coupon == null ? 0.00 : ((double)info.Coupon.CouponDiscount / 100);
                        discount.Text = String.Format("{0:C}", ((double)discountcheck) * ((double)info.SubTotal)).ToString();
                        subtotal.Text = "$" + info.SubTotal.ToString();
                        gst.Text = "$" + info.TaxAmount.ToString();
                        total.Text = String.Format("{0:C}", ((double)info.SubTotal + (double)info.TaxAmount) - ((double)discountcheck) * ((double)info.SubTotal)).ToString();
                    }, "Transaction Complete", "Payment Complete");
                }
            }
        }
    }
}