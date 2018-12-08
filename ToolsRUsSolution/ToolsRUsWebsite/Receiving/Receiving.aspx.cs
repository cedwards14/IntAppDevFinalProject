using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.Entities;
using ToolsRUsSystem.BLL;
using ToolsRUs.Data.DTOs;
#endregion

namespace ToolsRUsWebsite.Receiving
{
    public partial class Receving : System.Web.UI.Page
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
                    if (!User.IsInRole(SecurityRoles.Receiving))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                    else //user is logged in as receiving staff
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            PurchaseOrderController sysmgr = new PurchaseOrderController();
                            List<VendorPurchaseOrder> results = sysmgr.List_OutstandingOrders();
                            GridViewOutstandingOrders.DataSource = results;
                            GridViewOutstandingOrders.DataBind();
                        });
                    }
                }
            }
        }
        protected void OutstandingOrderList_ItemCommand(object sender, CommandEventArgs e)
        {
            int purchaseorderid = int.Parse(e.CommandArgument.ToString());
            Response.Redirect("ReceiveDetail.aspx?poid=" + purchaseorderid);
        }
    }
}