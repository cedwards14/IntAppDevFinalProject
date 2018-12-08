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
using ToolsRUs.Data.Entities;
using DMIT2018Common.UserControls;
using AppSecurity.BLL;
using Microsoft.AspNet.Identity.EntityFramework;
using AppSecurity.DAL;
#endregion

namespace ToolsRUsWebsite.Purchasing
{
    public partial class Purchasing : System.Web.UI.Page
    {
        static int vendorid;
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
                    if (!User.IsInRole(SecurityRoles.Purchasing))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
                vendorid = int.Parse(VendorDDL.SelectedValue);
                ReloadGrid();
            }

            string username = User.Identity.Name;
            ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(username);
            EmployeeName.Text = info.FullName;
        }

        protected void ReloadGrid()
        {
            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                List<PurchasingOrderDetail> info = sysmgr.List_PurchaseOrder_Get(vendorid);
                CurrentOrderGrid.DataSource = info;
                CurrentOrderGrid.DataBind();

                PurchaseOrderController psysmgr = new PurchaseOrderController();
                PurchasingOrder pinfo = psysmgr.Get_PurchaseOrder(vendorid);
                decimal subtotal = pinfo.Subtotal;
                decimal tax = pinfo.TaxAmount;
                decimal total = subtotal + tax;

                Subtotal.Text = pinfo.Subtotal.ToString("0.00");
                GST.Text = pinfo.TaxAmount.ToString("0.00");
                Total.Text = total.ToString();
                PO.Text = pinfo.PurchaseOrderID.ToString();
            });
        }

        protected void VendorStockListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int stockid = int.Parse(e.CommandArgument.ToString());
            int vendorid = int.Parse(VendorDDL.SelectedValue);
            int purchaseorderid = int.Parse(PO.Text.ToString());
            decimal price = decimal.Parse((e.Item.FindControl("PriceLabel") as Label).Text);

            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                sysmgr.Add_ItemToOrder(purchaseorderid, stockid, price);
                List<PurchasingOrderDetail> info = sysmgr.List_PurchaseOrder_Get(vendorid);
                CurrentOrderGrid.DataSource = info;
                CurrentOrderGrid.DataBind();

                PurchaseOrderController psysmgr = new PurchaseOrderController();
                PurchasingOrder pinfo = psysmgr.Get_PurchaseOrder(vendorid);
                decimal subtotal = pinfo.Subtotal;
                decimal tax = pinfo.TaxAmount;
                decimal total = subtotal + tax;

                Subtotal.Text = pinfo.Subtotal.ToString("0.00");
                GST.Text = pinfo.TaxAmount.ToString("0.00");
                Total.Text = total.ToString("0.00");
            }, "Success", "Item Added");
        }

        protected void CurrentOrderGrid_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            int podetailid = int.Parse(e.CommandArgument.ToString());
            int vendorid = int.Parse(VendorDDL.SelectedValue);
            int purchaseorderid = int.Parse(PO.Text.ToString());

            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                sysmgr.Remove_ItemFromOrder(purchaseorderid, podetailid);
                List<PurchasingOrderDetail> info = sysmgr.List_PurchaseOrder_Get(vendorid);
                CurrentOrderGrid.DataSource = info;
                CurrentOrderGrid.DataBind();

                PurchaseOrderController psysmgr = new PurchaseOrderController();
                PurchasingOrder pinfo = psysmgr.Get_PurchaseOrder(vendorid);
                decimal subtotal = pinfo.Subtotal;
                decimal tax = pinfo.TaxAmount;
                decimal total = subtotal + tax;

                Subtotal.Text = pinfo.Subtotal.ToString("0.00");
                GST.Text = pinfo.TaxAmount.ToString("0.00");
                Total.Text = total.ToString("0.00");
            }, "Success", "Item Removed");
        }

        protected void FetchVendor_Click(object sender, EventArgs e)
        {
            string username = User.Identity.Name;
            int employeeid = ReturnEmployeeID(username);
            int vendorid = int.Parse(VendorDDL.SelectedValue);
            if (vendorid == -1)
            {
                MessageUserControl.ShowInfo("Error", "Select a vendor");
            }
            else
            {

                MessageUserControl.TryRun(() =>
                {

                    //call controllers
                    PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                    PurchaseOrderController psysmgr = new PurchaseOrderController();
                    VendorController vsysmgr = new VendorController();

                    //get purchaseorderdetails
                    List<PurchasingOrderDetail> info = sysmgr.List_PurchaseOrder_Get(vendorid);
                    if (info.Count == 0)
                    {
                        psysmgr.Create_PurchaseOrder(vendorid, employeeid);
                        info = sysmgr.List_PurchaseOrder_Get(vendorid);
                    }
                    CurrentOrderGrid.DataSource = info;
                    CurrentOrderGrid.DataBind();

                    //get vendor info

                    VendorInfo vinfo = vsysmgr.Get_Vendor_Info(vendorid);
                    Address.Text = vinfo.Address.ToString();
                    City.Text = vinfo.CityProvince.ToString();
                    PostalCode.Text = vinfo.PostalCode.ToString();
                    Phone.Text = vinfo.Phone.ToString();

                    //get prices

                    PurchasingOrder pinfo = psysmgr.Get_PurchaseOrder(vendorid);
                    decimal subtotal = pinfo.Subtotal;
                    decimal tax = pinfo.TaxAmount;
                    decimal total = subtotal + tax;

                    Subtotal.Text = pinfo.Subtotal.ToString("0.00");
                    GST.Text = pinfo.TaxAmount.ToString("0.00");
                    Total.Text = total.ToString("0.00");
                    PO.Text = pinfo.PurchaseOrderID.ToString();

                }, "Success", "Showing details");
            }
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                CurrentOrderGrid.DataSource = null;
                CurrentOrderGrid.DataBind();
                VendorStockListView.DataSource = null;
                VendorStockListView.DataBind();
                VendorDDL.SelectedIndex = -1;
                Subtotal.Text = "0.00";
                GST.Text = "0.00";
                Total.Text = "0.00";
                PO.Text = "";
                Address.Text = "";
                City.Text = "";
                PostalCode.Text = "";
                Phone.Text = "";

            }, "Success", "Fields reset.");
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            int vendorid = int.Parse(VendorDDL.SelectedValue);
            bool isEmpty = false;
            List<PurchasingOrderDetail> purchaseorderdetails = new List<PurchasingOrderDetail>();

            if (CurrentOrderGrid.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("Update Failed", "Please select an order first.");
            }
            else
            {
                foreach (GridViewRow row in CurrentOrderGrid.Rows)
                {
                    string quantity = (row.FindControl("QuantityToOrder") as TextBox).Text;
                    string price = (row.FindControl("Price") as TextBox).Text;
                    if (string.IsNullOrEmpty(quantity))
                    {
                        isEmpty = true;
                        MessageUserControl.ShowInfo("Update Failed", "Quantity cannot be empty");
                        break;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(price))
                        {
                            isEmpty = true;
                            MessageUserControl.ShowInfo("Update Failed", "Price cannot be empty");
                            break;
                        }
                        else
                        {
                            if (quantity == "0")
                            {
                                MessageUserControl.ShowInfo("Update Failed", "Quantity cannot be 0");
                                break;
                            }
                            else
                            {
                                if (price == "0")
                                {
                                    MessageUserControl.ShowInfo("Update Failed", "Price cannot be 0");
                                    break;
                                }
                                else
                                {

                                    for (int index = 0; index < CurrentOrderGrid.Rows.Count; index++)
                                    {
                                        PurchasingOrderDetail tempList = new PurchasingOrderDetail();
                                        tempList.SID = int.Parse((CurrentOrderGrid.Rows[index].FindControl("SID") as Label).Text);
                                        tempList.Description = (CurrentOrderGrid.Rows[index].FindControl("Description") as Label).Text;
                                        tempList.QoH = int.Parse((CurrentOrderGrid.Rows[index].FindControl("QuantityOnHand") as Label).Text);
                                        tempList.QoO = int.Parse((CurrentOrderGrid.Rows[index].FindControl("QuantityOnOrder") as Label).Text);
                                        tempList.RoL = int.Parse((CurrentOrderGrid.Rows[index].FindControl("ReOrder") as Label).Text);
                                        tempList.QtO = int.Parse((CurrentOrderGrid.Rows[index].FindControl("QuantityToOrder") as TextBox).Text);
                                        tempList.Price = decimal.Parse((CurrentOrderGrid.Rows[index].FindControl("Price") as TextBox).Text);

                                        purchaseorderdetails.Add(tempList);
                                    }

                                    if (CurrentOrderGrid.Rows.Count == 0)
                                    {
                                        MessageUserControl.ShowInfo("Warning", "Select an order, or order no longer exists");
                                    }
                                    else
                                    {
                                        MessageUserControl.TryRun(() =>
                                        {
                                            int orderid = int.Parse(PO.Text.ToString());
                                            PurchaseOrderController psysmgr = new PurchaseOrderController();
                                            PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                                            sysmgr.Update_PurchaseOrderDetail(orderid, purchaseorderdetails);
                                            List<PurchasingOrderDetail> info = sysmgr.List_PurchaseOrder_Get(vendorid);

                                            CurrentOrderGrid.DataSource = info;
                                            CurrentOrderGrid.DataBind();

                                            PurchasingOrder pinfo = psysmgr.Get_PurchaseOrder(vendorid);
                                            decimal subtotal = pinfo.Subtotal;
                                            decimal tax = pinfo.TaxAmount;
                                            decimal total = subtotal + tax;

                                            Subtotal.Text = pinfo.Subtotal.ToString("0.00");
                                            GST.Text = pinfo.TaxAmount.ToString("0.00");
                                            Total.Text = total.ToString("0.00");

                                        }, "Update", "Update successful!");
                                    }
                                }
                            }
                            #region change back to original
                            //else
                            //{

                            //	for (int index = 0; index < CurrentOrderGrid.Rows.Count; index++)
                            //	{
                            //		PurchasingOrderDetail tempList = new PurchasingOrderDetail();
                            //		tempList.SID = int.Parse((CurrentOrderGrid.Rows[index].FindControl("SID") as Label).Text);
                            //		tempList.Description = (CurrentOrderGrid.Rows[index].FindControl("Description") as Label).Text;
                            //		tempList.QoH = int.Parse((CurrentOrderGrid.Rows[index].FindControl("QuantityOnHand") as Label).Text);
                            //		tempList.QoO = int.Parse((CurrentOrderGrid.Rows[index].FindControl("QuantityOnOrder") as Label).Text);
                            //		tempList.RoL = int.Parse((CurrentOrderGrid.Rows[index].FindControl("ReOrder") as Label).Text);
                            //		tempList.QtO = int.Parse((CurrentOrderGrid.Rows[index].FindControl("QuantityToOrder") as TextBox).Text);
                            //		tempList.Price = decimal.Parse((CurrentOrderGrid.Rows[index].FindControl("Price") as TextBox).Text);

                            //		purchaseorderdetails.Add(tempList);
                            //	}

                            //	if (CurrentOrderGrid.Rows.Count == 0)
                            //	{
                            //		MessageUserControl.ShowInfo("Warning", "Select an order, or order no longer exists");
                            //	}
                            //	else
                            //	{
                            //		MessageUserControl.TryRun(() =>
                            //		{
                            //			int orderid = int.Parse(PO.Text.ToString());
                            //			PurchaseOrderController psysmgr = new PurchaseOrderController();
                            //			PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                            //			sysmgr.Update_PurchaseOrderDetail(orderid, purchaseorderdetails);
                            //			List<PurchasingOrderDetail> info = sysmgr.List_PurchaseOrder_Get(vendorid);

                            //			CurrentOrderGrid.DataSource = info;
                            //			CurrentOrderGrid.DataBind();

                            //			PurchasingOrder pinfo = psysmgr.Get_PurchaseOrder(vendorid);
                            //			decimal subtotal = pinfo.Subtotal;
                            //			decimal tax = pinfo.TaxAmount;
                            //			decimal total = subtotal + tax;

                            //			Subtotal.Text = pinfo.Subtotal.ToString("0.00");
                            //			GST.Text = pinfo.TaxAmount.ToString("0.00");
                            //			Total.Text = total.ToString("0.00");

                            //		}, "Update", "Update successful!");
                            //	}
                            #endregion
                        }
                    }
                }
            }
        }



        protected void Delete_Click(object sender, EventArgs e)
        {
            int vendorid = int.Parse(VendorDDL.SelectedValue);

            List<PurchaseOrderDetail> itemstodelete = new List<PurchaseOrderDetail>();

            if (PO.Text == "")
            {
                MessageUserControl.ShowInfo("Warning", "Select an order, or order no longer exists");
            }
            else
            {
                for (int index = 0; index < CurrentOrderGrid.Rows.Count; index++)
                {
                    PurchaseOrderDetail tempList = new PurchaseOrderDetail();
                    tempList.PurchaseOrderDetailID = int.Parse((CurrentOrderGrid.Rows[index].FindControl("POID") as Label).Text);

                    itemstodelete.Add(tempList);
                }

                MessageUserControl.TryRun(() =>
                {
                    int orderid = int.Parse(PO.Text.ToString());
                    PurchaseOrderController sysmgr = new PurchaseOrderController();
                    sysmgr.Delete_PurchaseOrder(orderid);
                    PurchaseOrderDetailController psysmgr = new PurchaseOrderDetailController();
                    List<PurchasingOrderDetail> info = psysmgr.List_PurchaseOrder_Get(vendorid);
                    CurrentOrderGrid.DataSource = info;
                    CurrentOrderGrid.DataBind();
                    VendorStockListView.DataSource = null;
                    VendorStockListView.DataBind();
                    VendorDDL.SelectedIndex = -1;

                    Subtotal.Text = "0.00";
                    GST.Text = "0.00";
                    Total.Text = "0.00";
                    PO.Text = "";
                    Address.Text = "";
                    City.Text = "";
                    PostalCode.Text = "";
                    Phone.Text = "";

                }, "Update", "Delete successful!");
            }

        }

        protected void Place_Click(object sender, EventArgs e)
        {
            if (CurrentOrderGrid.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("Place Order Failed", "Please select an order first or Order is empty.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    int vendorid = int.Parse(VendorDDL.SelectedValue);
                    int orderid = int.Parse(PO.Text.ToString());
                    PurchaseOrderController sysmgr = new PurchaseOrderController();
                    sysmgr.Place_PurchaseOrder(vendorid, orderid);
                    CurrentOrderGrid.DataSource = null;
                    CurrentOrderGrid.DataBind();
                    Subtotal.Text = "0.00";
                    GST.Text = "0.00";
                    Total.Text = "0.00";
                    PO.Text = "";
                    Address.Text = "";
                    City.Text = "";
                    PostalCode.Text = "";
                    Phone.Text = "";
                    VendorDDL.SelectedIndex = -1;
                }, "Success", "Order has been placed. Press fetch to create new order.");
            }
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

    }
}