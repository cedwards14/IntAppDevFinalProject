using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.Entities;
using ToolsRUsSystem.BLL;
using ToolsRUs.Data.POCOs;
using AppSecurity.BLL;
using Microsoft.AspNet.Identity.EntityFramework;
using AppSecurity.DAL;
using ToolsRUs.Data.Entities;
#endregion

namespace ToolsRUsWebsite.Rentals
{
    public partial class Rentals : System.Web.UI.Page
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

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Clear();
            string phonenumber = AreaCode.Text + "." + Phone1.Text + "." + Phone2.Text;
            MessageUserControl.TryRun(() =>
            {
                CustomerController sysmgr = new CustomerController();
                List<CustomerInfo> results = sysmgr.List_CustomerByPhone(phonenumber);
                if (results.Count == 0)
                {
                    MessageUserControl.ShowInfo("Please try a different Phonenumber");
                }
                else
                {
                    if (results.Count >= 6)
                    {
                        MessageUserControl.ShowInfo("Please narrow your search");
                    }
                    else
                    {
                        CustomerGV.DataSource = results;
                        CustomerGV.DataBind();
                    }
                }
            });
        }

        protected void Fetch_Command(object sender, CommandEventArgs e)
        {

            int customerid = int.Parse(e.CommandArgument.ToString());
            MessageUserControl.TryRun(() =>
            {
                CustomerController sysmgr = new CustomerController();
                Customer result = sysmgr.Customer_Get(customerid);

                customerID.Text = result.CustomerID.ToString();
                customername.Text = result.LastName + ", " + result.FirstName;
                address.Text = result.Address;
                city.Text = result.City;
            });
        }


        protected void AvailableRentalEquipment_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string username = User.Identity.Name;



            if (string.IsNullOrEmpty(customerID.Text))
            {
                MessageUserControl.ShowInfo("No Customer has been selelected");
            }
            else
            {
                if (string.IsNullOrEmpty(creditcard.Text))
                {
                    MessageUserControl.ShowInfo("Please enter a creditcard");

                }

                if (creditcard.Text.Length < 16)
                {
                    MessageUserControl.ShowInfo("not a valid creditcard");
                }
                else
                {
                    long creditcheck = 0;
                    if (!long.TryParse(creditcard.Text, out creditcheck))
                    {
                        MessageUserControl.ShowInfo("Nice Try!");
                    }
                    else
                    {
                        int custID = int.Parse(customerID.Text);
                        int employeeid = ReturnEmployeeID(username);
                        int equipmentRID = int.Parse(e.CommandArgument.ToString());
                        string credit = creditcard.Text;
                        if (employeeid == 0)
                        {
                            MessageUserControl.ShowInfo("You are not logged in");
                        }
                        else
                        {

                            MessageUserControl.TryRun(() =>
                            {
                                RentalDetailController sysmgr = new RentalDetailController();

                                sysmgr.Add_RentalDetailToRental(employeeid, equipmentRID, custID, credit);

                                List<CurrentRental> myCurrentRental = sysmgr.List_EquipmentForRental(custID, employeeid, equipmentRID);
                                RentalGV.DataSource = myCurrentRental;
                                RentalGV.DataBind();
                                AvailableRentalEquipment.DataBind();

                            }, "Rental Equipment Added", "Your Equipment has been added to your order");

                        }
                    }
                }

            }

        }




            public void AllowRentalGVPAging()
        {
            int Detailid = int.Parse((RentalGV.Rows[0].FindControl("DetailID") as Label).Text);
            RentalDetailController testeroni = new RentalDetailController();
            List<CurrentRental> mylist = testeroni.mecheating(Detailid);
            RentalGV.DataSource = mylist;
            RentalGV.DataBind();
        }


        protected void CurrentRentalRemove_Command(object sender, CommandEventArgs e)
        {
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');
            int detail = int.Parse(arg[1]);
            int equip = int.Parse(arg[0]);
            string username = User.Identity.Name;
            int employeeid = ReturnEmployeeID(username);
            int custID = int.Parse(customerID.Text);

            MessageUserControl.TryRun(() =>
            {
                RentalDetailController sysmgr = new RentalDetailController();
                sysmgr.Remove_RentalEquipment(employeeid, equip, custID, detail);


                List<CurrentRental> myCurrentRental = sysmgr.List_EquipmentForRental(custID, employeeid, equip);
                RentalGV.DataSource = myCurrentRental;
                RentalGV.DataBind();
                AvailableRentalEquipment.DataBind();

            }, "Transaction Complete", "The item has been removed");

        }//EOM


        #region Get EmployeeID
        public int ReturnEmployeeID(string username)
        {
            ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(username);
            int employeeid = info.EmployeeID;

            return employeeid;
        }
        #endregion

        protected void checkbutton_Click(object sender, EventArgs e)
        {

            if (RentalGV.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("Please start a rental");
            }
            else
            {
                if (string.IsNullOrEmpty(coupon.Text))
                {
                    MessageUserControl.ShowInfo("Please enter the coupon code");
                }
                else
                {
                    string couponIDValue = coupon.Text;

                    CouponController sysmgr = new CouponController();
                    Coupon aCoupon = sysmgr.Coupon_GetByCouponIDValue(couponIDValue);

                    if (aCoupon == null)
                    {
                        MessageUserControl.ShowInfo("Please enter a valid Coupon");
                    }
                    else
                    {
                        DateTime today = new DateTime();
                        today = DateTime.Today;
                        if (today < aCoupon.StartDate)
                        {
                            MessageUserControl.ShowInfo("Coupon is not valid yet");
                        }
                        else
                        {
                            if (today > aCoupon.EndDate)
                            {
                                MessageUserControl.ShowInfo("Coupon is no longer valid");
                            }
                            else
                            {
                                MessageUserControl.TryRun(() =>
                            {
                                int couponID = aCoupon.CouponID;

                                int rentalDetailId = int.Parse((RentalGV.Rows[0].FindControl("DetailID") as Label).Text);
                                RentalDetailController sysmgr2 = new RentalDetailController();
                                sysmgr2.Coupon_Rental(rentalDetailId, couponID);

                            }, "Transaction Complete", "Your Coupon has been added to your rental!");
                            }
                        }
                    }
                }//////
            }
        }

        protected void cancelbutton_Click(object sender, EventArgs e)
        {
            if (RentalGV.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("There is no rental to Cancel");

            }
            else
            {
                string username = User.Identity.Name;
                int empId = ReturnEmployeeID(username);
                int custId = int.Parse(customerID.Text);
                int rentalDetailId = int.Parse((RentalGV.Rows[0].FindControl("DetailID") as Label).Text);
                MessageUserControl.TryRun(() =>
                {
                    RentalDetailController sysmgr = new RentalDetailController();
                    sysmgr.Cancel_Rental(empId, custId, rentalDetailId);
                    AvailableRentalEquipment.DataBind();
                    RentalGV.DataBind();
                }, "Transaction Complete", "The Rental has been cancelled!");
                CancelClear();
            }
        }

    

        protected void RentalGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            GridView RentalGV = (GridView)sender;
            RentalGV.PageIndex = e.NewPageIndex;
            AllowRentalGVPAging();
        }


        public void Clear()
        {
            CustomerGV.DataSource = null;
            CustomerGV.DataBind();
            if(RentalGV.Rows.Count >0)
            {
                RentalGV.DataSource = null;
                RentalGV.DataBind();
            }

            customerID.Text = "";
            customername.Text = "";
            address.Text = "";
            city.Text = "";
            creditcard.Text = "";
            coupon.Text = "";
        }

        public void CancelClear()
        {
            Clear();
            AreaCode.Text = "";
            Phone1.Text = "";
            Phone2.Text = "";
        }

    }
}