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
using System.Data;
using DMIT2018Common.UserControls;
using ToolsRUs.Data.DTOs;
#endregion

namespace ToolsRUsWebsite.Receiving
{
    public partial class ReceiveDetail : System.Web.UI.Page
    {
        static int purchaseorderid;
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
                    if (!User.IsInRole(SecurityRoles.Receiving))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                    else //user is logged in as receiving staff
                    {
                        //check that an outstanding order has been selected frm the receiving screen
                        if (Request.QueryString["poid"] == null)
                        {
                            Response.Redirect("Receiving.aspx");
                        }
                        else if (!IsPostBack)
                        {
                            purchaseorderid = int.Parse(Request.QueryString["poid"]);
                            LoadUnorderedItemGridView();
                            LoadReceiveDetailGridView();
                        }
                    }
                }
            }
        }

        protected void LoadReceiveDetailGridView()
        {
            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderController sysmgr = new PurchaseOrderController();
                VendorPurchaseOrder result = sysmgr.Get_VendorPurchaseOrder(purchaseorderid);
                PurchaseOrderNumber.Text = "Purchase Order Number: " + result.PurchaseOrderNumber.ToString();
                VendorName.Text = "Vendor: " + result.VendorName;
                VendorPhone.Text = "Vendor Contact Phone: " + result.Phone;
                GridViewReceiveOrderDetail.DataSource = result.VendorOrderDetail;
                GridViewReceiveOrderDetail.DataBind();
            });
        }

        protected void LoadUnorderedItemGridView()
        {
            MessageUserControl.TryRun(() =>
            {
                UnorderedPurchaseItemCartController sysmgr = new UnorderedPurchaseItemCartController();
                List<UnorderedPurchaseItemCart> results = sysmgr.List_UnorderedPurchaseItemCart(purchaseorderid);
                GridViewUnorderedPurchaseItem.DataSource = results;
                GridViewUnorderedPurchaseItem.DataBind();
                CheckForEmptyGV();
            });
        }

        protected void CheckForEmptyGV()
        {
            //show only the "add item" footer if there are no unordered items created yet
            if (GridViewUnorderedPurchaseItem.Rows.Count == 0)
            {
                DataTable emptyTable = new DataTable();
                emptyTable.Columns.Add(new DataColumn("CartID", typeof(int)));
                emptyTable.Columns.Add(new DataColumn("Description"));
                emptyTable.Columns.Add(new DataColumn("VendorStockNumber"));
                emptyTable.Columns.Add(new DataColumn("Quantity", typeof(int)));
                DataRow emptyRow = emptyTable.NewRow();
                emptyTable.Rows.Add(emptyRow);
                GridViewUnorderedPurchaseItem.DataSource = emptyTable;
                GridViewUnorderedPurchaseItem.DataBind();
                GridViewUnorderedPurchaseItem.Rows[0].Visible = false;
            }
        }

        protected void UnorderedPurchaseItemCart_ItemCommand(object sender, CommandEventArgs e)
        {
            switch (e.CommandArgument.ToString())
            {
                case "Add":
                    string description = (GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxDescription") as TextBox).Text;
                    string vendorstocknumber = (GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxVendorStockNumber") as TextBox).Text;
                    int quantity;
                    bool quantityInt = int.TryParse((GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxQuantity") as TextBox).Text, out quantity);

                    if (description == "")
                    {
                        MessageUserControl.ShowInfo("Error adding unordered item", "Item description is required");
                    }
                    else
                    {
                        if (vendorstocknumber == "")
                        {
                            MessageUserControl.ShowInfo("Error adding unordered item", "Vendor stock number is required");
                        }
                        else
                        {
                            if (!quantityInt)
                            {
                                MessageUserControl.ShowInfo("Error adding unordered item", "An integer quantity is required");
                            }
                            else
                            {
                                if (quantity <= 0)
                                {
                                    MessageUserControl.ShowInfo("Error adding unordered item", "Item quantity must be greater than zero");
                                }
                                else
                                {
                                    MessageUserControl.TryRun(() =>
                                    {
                                        UnorderedPurchaseItemCart newItem = new UnorderedPurchaseItemCart();
                                        newItem.PurchaseOrderID = purchaseorderid;
                                        newItem.Description = description;
                                        newItem.VendorStockNumber = vendorstocknumber;
                                        newItem.Quantity = int.Parse((GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxQuantity") as TextBox).Text);
                                        UnorderedPurchaseItemCartController sysmgr = new UnorderedPurchaseItemCartController();
                                        sysmgr.Add_UnorderedPurchaseItemCart(newItem);
                                        LoadUnorderedItemGridView();
                                    }, "Added", "Cart item has been added");
                                }
                            }
                        }
                    }
                    break;
                case "Clear":
                    (GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxDescription") as TextBox).Text = "";
                    (GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxVendorStockNumber") as TextBox).Text = "";
                    (GridViewUnorderedPurchaseItem.FooterRow.FindControl("TextBoxQuantity") as TextBox).Text = "0";
                    break;
                default:
                    //delete unordered item entry
                    int cartid = int.Parse(e.CommandArgument.ToString());
                    MessageUserControl.TryRun(() =>
                    {
                        UnorderedPurchaseItemCartController sysmgr = new UnorderedPurchaseItemCartController();
                        sysmgr.Delete_UnorderedPurchaseItemCart(cartid);
                        LoadUnorderedItemGridView();
                    }, "Removed", "Cart item has been removed");
                    break;
            }
        }

        protected void ForceClose_Click(object sender, EventArgs e)
        {
            string closeReason = ForceCloseReason.Text;
            if (string.IsNullOrEmpty(closeReason))
            {
                MessageUserControl.ShowInfo("No reason supplied", "A reason is required to force close an order");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    PurchaseOrderController sysmgr = new PurchaseOrderController();
                    sysmgr.ForceClose_PurchaseOrder(purchaseorderid, closeReason);
                    Response.Redirect("Receiving.aspx");
                });
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            //clear all entry fields
            for (int i = 0; i < GridViewReceiveOrderDetail.Rows.Count; i++)
            {
                (GridViewReceiveOrderDetail.Rows[i].FindControl("QuantityReceived") as TextBox).Text = "0";
                (GridViewReceiveOrderDetail.Rows[i].FindControl("QuantityReturned") as TextBox).Text = "0";
                (GridViewReceiveOrderDetail.Rows[i].FindControl("ReturnReason") as TextBox).Text = "";
            }
            MessageUserControl.TryRun(() =>
            {
                UnorderedPurchaseItemCartController sysmgr = new UnorderedPurchaseItemCartController();
                //delete any cart items that have been added
                if ((GridViewUnorderedPurchaseItem.Rows[0].FindControl("CartID") as Label).Text != "")
                {
                    for (int i = 0; i < GridViewUnorderedPurchaseItem.Rows.Count; i++)
                    {
                        int cartid = int.Parse((GridViewUnorderedPurchaseItem.Rows[i].FindControl("CartID") as Label).Text);
                        sysmgr.Delete_UnorderedPurchaseItemCart(cartid);
                    }
                }
                Response.Redirect("Receiving.aspx");
            });
        }
        protected void Receive_Click(object sender, EventArgs e)
        {
            List<ReturnedOrderDetail> returnedorderdetails = new List<ReturnedOrderDetail>();
            List<ReceiveOrderDetail> receiveorderdetails = new List<ReceiveOrderDetail>();
            List<string> reasons = new List<string>();
            for (int i = 0; i < GridViewReceiveOrderDetail.Rows.Count; i++)
            {
                int received, returned, purchaseorderdetailid;
                string stockitemdescription, vendorstocknumber;
                bool receivedInt = int.TryParse((GridViewReceiveOrderDetail.Rows[i].FindControl("QuantityReceived") as TextBox).Text, out received);
                bool returnedInt = int.TryParse((GridViewReceiveOrderDetail.Rows[i].FindControl("QuantityReturned") as TextBox).Text, out returned);
                purchaseorderdetailid = int.Parse((GridViewReceiveOrderDetail.Rows[i].FindControl("PurchaseOrderDetailID") as Label).Text);
                stockitemdescription = (GridViewReceiveOrderDetail.Rows[i].FindControl("StockItemDescription") as Label).Text;
                vendorstocknumber = (GridViewReceiveOrderDetail.Rows[i].FindControl("VendorStockNumber") as Label).Text;
                string reason = (GridViewReceiveOrderDetail.Rows[i].FindControl("ReturnReason") as TextBox).Text;

                if (!receivedInt || !returnedInt)
                {
                    reasons.Add("Received and returned quantities must be integers. Use \"0\" if the quantity is zero");
                }
                else
                {
                    if (received < 0 || returned < 0)
                    {
                        reasons.Add("Received and returned quantities must be positive or zero. Use \"0\" if the quantity is zero");
                    }
                    else
                    {
                        if ((returned != 0 && reason == "") || (returned == 0 && reason != ""))
                        {
                            reasons.Add("Each returned item requires a supplied reason. Reasons must be recorded only for returned items");
                        }
                        else
                        {
                            if (int.Parse((GridViewReceiveOrderDetail.Rows[i].FindControl("QuantityOutstanding") as Label).Text) < received)
                            {
                                reasons.Add("The quantity received for an order must be less than or equal to the quantity outstanding");
                            }
                            else
                            {
                                if (returned > 0)
                                {
                                    ReturnedOrderDetail returnedorderentry = new ReturnedOrderDetail();
                                    returnedorderentry.PurchaseOrderDetailID = purchaseorderdetailid;
                                    returnedorderentry.Quantity = returned;
                                    returnedorderentry.Reason = reason;
                                    returnedorderentry.ItemDescription = stockitemdescription;
                                    returnedorderentry.VendorStockNumber = vendorstocknumber;

                                    returnedorderdetails.Add(returnedorderentry);
                                }
                                if (received > 0)
                                {
                                    ReceiveOrderDetail receiveorderentry = new ReceiveOrderDetail();
                                    receiveorderentry.PurchaseOrderDetailID = purchaseorderdetailid;
                                    receiveorderentry.QuantityReceived = received;

                                    receiveorderdetails.Add(receiveorderentry);
                                }
                            }
                        }
                    }
                }
            }
            //check that there are entries for at least 1 item received or returned
            if (reasons.Count > 0)
            {
                string messageToShow = "";
                foreach (string reasonMessage in reasons)
                {
                    messageToShow += reasonMessage + "<br />";
                }
                MessageUserControl.ShowInfo("Error processing receive", messageToShow);
            }
            else if ((receiveorderdetails.Count > 0 || returnedorderdetails.Count > 0 || ((GridViewUnorderedPurchaseItem.Rows[0].FindControl("CartID") as Label).Text != "")) && reasons.Count == 0)
            {
                MessageUserControl.TryRun(() =>
                {
                    ReceiveReturnOrderController sysmgr = new ReceiveReturnOrderController();
                    sysmgr.Add_ReceiveReturnOrders(purchaseorderid, returnedorderdetails, receiveorderdetails);

                    Response.Redirect("Receiving.aspx");
                });
            }
            else
            {
                MessageUserControl.ShowInfo("No data entered", "Please enter the number of received and/or returned items");
            }
            //check that there are no empty fields
            //check that the order hasn't been received
            //loop through the gridview, pulling entered data from textboxes in to purchaseorderstockdetail
            //create ReceiveOrders entry, 
            //create ReceiveOrderDetail entry,
            //create ReturnedOrderDetail entry for both UnorderedPurchaseItems and for damaged / refused items update StockItems quantities, 
            //close PurchaseOrder if all quantities outstanding have been received, 
            //empty cart

            //only add entry for receive or return if quantity != 0

        }//eom
    }
}