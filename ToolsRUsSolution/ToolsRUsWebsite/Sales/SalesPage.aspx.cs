using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ToolsRUsSystem.BLL;
using ToolsRUs.Data.POCOs;
using AppSecurity.Entities;
using AppSecurity.BLL;
using Microsoft.AspNet.Identity.EntityFramework;
using AppSecurity.DAL;
using System.Globalization;
using ToolsRUs.Data.Entities;
#endregion

namespace ToolsRUsWebsite.Sales
{
    public partial class SalesPage : System.Web.UI.Page
    {
        protected static string tabPanel;
        protected void Page_Load(object sender, EventArgs e)
        {

            //set count of all label            
            StockItemController sysmgr = new StockItemController();
            string count = sysmgr.Select_ItemCount().ToString();
            AllCategoryItemCount.Text = count;

            if(!IsPostBack && User.IsInRole(SecurityRoles.Sales))
            {
                UpdateShoppingCart();
                tabPanel = "#panel1";
            }
            


        }
        protected void CouponButton_OnClick(object sender, EventArgs e)
        {
            tabPanel = "#panel3";
            string couponIdValue = CouponTextBox.Text;
            if(CheckoutListView.Items.Count() < 1)
            {
                MessageUserControl.ShowInfo("Can't apply a coupon without an order");
            }
            else
            {
                if (string.IsNullOrEmpty(couponIdValue))
                {
                    MessageUserControl.ShowInfo("Must add a coupon ID value");
                }
                else
                {
                    CouponController sysmgr = new CouponController();
                    Coupon coupon = sysmgr.Coupon_GetByCouponIDValue(couponIdValue);
                    if (coupon == null)
                    {
                        MessageUserControl.ShowInfo("Not a valid coupon ID");
                    }
                    else
                    {
                        //check to see if coupons end date is before today
                        int diff = DateTime.Compare(coupon.EndDate, DateTime.Now.Date);

                        if (diff < 0)
                        {
                            MessageUserControl.ShowInfo("Coupon no longer valid");
                        }
                        else
                        {
                            diff = DateTime.Compare(coupon.StartDate, DateTime.Now.Date);
                            if(diff > 0)
                            {
                                MessageUserControl.ShowInfo("Coupon not yet available");
                            }
                            else
                            {
                                MessageUserControl.TryRun(() =>
                                {
                                    double subtotal = double.Parse(SubtotalCheckoutLabel.Text, NumberStyles.Currency);
                                    double discount = Convert.ToDouble(coupon.CouponDiscount) / 100;
                                    CouponAmountLabel.Text = String.Format("{0:C}", (subtotal * discount));
                                    CouponDiv.Visible = true;
                                    CouponId.Text = coupon.CouponIDValue;
                                    UpdateShoppingCart();                                    
                                }, "Coupon", "Coupon has been added");
                            }
                        }
                    }
                }
            }
            

        }
        public void UpdateShoppingCart()
        {
            string username = User.Identity.Name;
            ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));


            EmployeeInfo info = secmgr.User_GetEmployee(username);
            int employeeID = info.EmployeeID;


            SalesController sysmgr2 = new SalesController();
            ShoppingCartIDLabel.Text = sysmgr2.ShoppingCartIdByEmployeeID(employeeID).ToString();
            ShoppingCartItemListView.DataBind();
            CheckoutListView.DataBind();
            if (ShoppingCartItemListView.Items.Count() > 0)
            {
                double subTotal = 0;
                foreach (ListViewItem itemRow in ShoppingCartItemListView.Items)
                {
                    Label itemPurchasePrice = itemRow.FindControl("PurchasePriceLabel") as Label;
                    Label itemTotal = itemRow.FindControl("shoppingCartItemTotalLabel") as Label;

                    TextBox quantityPicketTB = itemRow.FindControl("QuantityTextBox") as TextBox;

                    double purchasePrice = double.Parse(itemPurchasePrice.Text, NumberStyles.Currency);
                    double quantity = double.Parse(quantityPicketTB.Text, NumberStyles.Currency);
                    itemTotal.Text = String.Format("{0:C}", (purchasePrice * quantity));
                    subTotal += (purchasePrice * quantity);
                }
                cartTotalLabel.Text = "Total: " + String.Format("{0:C}", subTotal);
                cartTotalLabel.Font.Bold = true;

                //update the checkout
                foreach (ListViewItem itemRow in CheckoutListView.Items)
                {
                    Label itemPurchasePrice = itemRow.FindControl("PurchasePriceLabel") as Label;
                    Label itemTotal = itemRow.FindControl("CheckoutItemTotalLabel") as Label;

                    Label quantityPicketTB = itemRow.FindControl("QuantityTextBox") as Label;

                    double purchasePrice = double.Parse(itemPurchasePrice.Text, NumberStyles.Currency);
                    double quantity = double.Parse(quantityPicketTB.Text, NumberStyles.Currency);
                    itemTotal.Text = String.Format("{0:C}", (purchasePrice * quantity));
                }
                SubtotalCheckoutLabel.Text = String.Format("{0:C}", subTotal);
                TaxTotalLabel.Text = String.Format("{0:C}", (subTotal * .05));

                
                try
                {
                    CouponController sysmgr = new CouponController();
                    Coupon coupon = sysmgr.Coupon_GetByCouponIDValue(CouponId.Text);
                    double subtotal = double.Parse(SubtotalCheckoutLabel.Text, NumberStyles.Currency);
                    double discount = Convert.ToDouble(coupon.CouponDiscount) / 100;
                    CouponAmountLabel.Text = String.Format("{0:C}", (subtotal * discount));
                    
                }
                catch
                {

                }
                
                if (ShoppingCartItemListView.Items.Count() > 0)
                {
                    CouponDiv.Visible = true;
                }
                if (ShoppingCartItemListView.Items.Count() < 1 || CouponId.Text == "")
                {
                    CouponDiv.Visible = false;
                }
                if (CouponId.Text == "")
                {
                    TotalCheckoutLabel.Text = String.Format("{0:C}", (subTotal * 1.05));
                }
                else
                {
                    TotalCheckoutLabel.Text = String.Format("{0:C}", ((subTotal * 1.05) - double.Parse(CouponAmountLabel.Text, NumberStyles.Currency)));
                }
                
            }
        }
        protected void Category_Button(object sender, EventArgs e)
        {
            tabPanel = "#panel1";
            LinkButton btn = (LinkButton)(sender);
            string yourValue = btn.CommandArgument;
            Category.Text = yourValue;
            ItemListODS.DataBind();

        }
        protected void Cancel_Button(object sender, EventArgs e)
        {
            tabPanel = "#panel1";
            if (!Request.IsAuthenticated)
            {
                MessageUserControl.ShowInfo("Not logged in");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    string username = User.Identity.Name;
                    ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));


                    EmployeeInfo info = secmgr.User_GetEmployee(username);
                    int employeeID = info.EmployeeID;

                    SalesController sysmgr = new SalesController();
                    sysmgr.RemoveCartAndItems(employeeID);
                    UpdateShoppingCart();

                    SubtotalCheckoutLabel.Text = "";
                    TaxTotalLabel.Text = "";
                    TotalCheckoutLabel.Text = "";
                    cartTotalLabel.Text = "";
                    CouponTextBox.Text = "";
                    CouponId.Text = "";
                    CouponDiv.Visible = false;
                }, "Shopping", "Canceled");
            } 
        }


        protected void CartItemDelete_Button(object sender, EventArgs e)
        {
            tabPanel = "#panel2";
            LinkButton btn = (LinkButton)(sender);
            string yourValue = btn.CommandArgument;
            MessageUserControl.TryRun(() =>
            {
                ShoppingCartItemController sysmgr = new ShoppingCartItemController();
                sysmgr.ShoppingCartItem_Delete(int.Parse(yourValue));
                
            }, "Shopping Cart", "Item removed");
            UpdateShoppingCart();
            if(ShoppingCartItemListView.Items.Count() < 1)
            {
                SubtotalCheckoutLabel.Text = "";
                TaxTotalLabel.Text = "";
                TotalCheckoutLabel.Text = "";
                cartTotalLabel.Text = "";
                CouponDiv.Visible = false;
            }

        }
        protected void CartItemUpdate_Button(object sender, ListViewCommandEventArgs e)
        {
            tabPanel = "#panel2";
            Label cartItemIdLabel = (Label)e.Item.FindControl("StockItemIDLabel") as Label;
            int cartItemID = int.Parse(cartItemIdLabel.Text);

            TextBox tb = (TextBox)e.Item.FindControl("QuantityTextBox");

            string quantity = tb.Text.ToString();

            if (string.IsNullOrEmpty(quantity))
            {
                MessageUserControl.ShowInfo("Required Data", "You must provide a quantity");
            }
            else
            {
                int parsedValue;
                if (!int.TryParse(quantity, out parsedValue))
                {
                    MessageUserControl.ShowInfo("Wrong Data type", "This field can only accept integers");
                }
                else
                {
                    if (parsedValue < 1)
                    {
                        MessageUserControl.ShowInfo("Data Error", "Quantity must be greater than 0");
                    }
                    else
                    {
                        //Test to make sure it is less than the QOH
                        ShoppingCartItemController shoppingCartItemController = new ShoppingCartItemController();
                        ShoppingCartItem shoppingCartItem = shoppingCartItemController.GetShoppingCartItem_ByID(cartItemID);

                        StockItemController stockItemController = new StockItemController();       
                        StockItem stockItem = stockItemController.GetQOH_ByStockItemID(shoppingCartItem.StockItemID);
                        if(parsedValue > stockItem.QuantityOnHand)
                        {
                            MessageUserControl.ShowInfo("Data Error", "Not enough Quantity On Hand, QOH =" + stockItem.QuantityOnHand);
                        }
                        else
                        {
                            MessageUserControl.TryRun(() =>
                            {
                                ShoppingCartItemController sysmgr = new ShoppingCartItemController();
                                sysmgr.ShoppingCartItem_Update(cartItemID, parsedValue);
                                UpdateShoppingCart();
                            }, "Item Updated", "The item has been updated");
                        }
                    }
                }
            }

        }
        protected void AllCategory_Button(object sender, EventArgs e)
        {
            tabPanel = "#panel1";
            Category.Text = "-1";
            ItemListODS.DataBind();
        }
        protected void AddButton_OnClick(object sender, ListViewCommandEventArgs e)
        {
            tabPanel = "#panel1";
            int itemID = int.Parse(e.CommandArgument.ToString());

            TextBox tb = (TextBox)e.Item.FindControl("QuantityTextBox");
            string quantity = tb.Text.ToString();
            if (string.IsNullOrEmpty(quantity))
            {
                MessageUserControl.ShowInfo("Required Data", "You must provide a quantity");
            }
            else
            {
                int parsedValue;
                if (!int.TryParse(quantity, out parsedValue))
                {
                    MessageUserControl.ShowInfo("Wrong Data type", "This field can only accept integers");
                }
                else
                {
                    if (parsedValue < 1)
                    {
                        MessageUserControl.ShowInfo("Data Error", "Quantity must be greater than 0");
                    }
                    else
                    {
                        //get quantity on hand 
                        Label label = (Label)e.Item.FindControl("QuantityOnHandLabel");
                        int QOH = int.Parse(label.Text.ToString());


                        if (parsedValue > QOH)
                        {
                            MessageUserControl.ShowInfo("Data Error", "Quantity must be less than QOH");
                        }
                        else
                        {
                            MessageUserControl.TryRun(() =>
                            {
                                string username = User.Identity.Name;
                                ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));


                                EmployeeInfo info = secmgr.User_GetEmployee(username);
                                int employeeID = info.EmployeeID;


                                SalesController sysmgr = new SalesController();
                                sysmgr.AddToCart(employeeID, itemID, parsedValue);
                                ItemListODS.DataBind();

                                ShoppingCartIDLabel.Text = sysmgr.ShoppingCartIdByEmployeeID(employeeID).ToString();
                                ShoppingCartItemListView.DataBind();

                                UpdateShoppingCart();

                            }, "Item Added", "The item has been added to your cart");
                        }
                    }
                }
            }
        }
        protected void PlaceOrder_ButtonClick(object sender, EventArgs e)
        {
            tabPanel = "#panel1";
            if (!Request.IsAuthenticated)
            {
                MessageUserControl.ShowInfo("Log in to place orders");
            }
            else
            {
                if (ShoppingCartItemListView.Items.Count < 1)
                {
                    MessageUserControl.ShowInfo("Cannot place an empty order");
                }
                else
                {
                    MessageUserControl.TryRun(() =>
                    {

                        string username = User.Identity.Name;
                        ApplicationUserManager secmgr = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));


                        EmployeeInfo info = secmgr.User_GetEmployee(username);
                        int employeeID = info.EmployeeID;
                        string paymentType = RadioButtonList1.SelectedValue;

                        CouponController couponManager = new CouponController();
                        Coupon coupon = couponManager.Coupon_GetByCouponIDValue(CouponId.Text);
                        if (coupon == null)
                        {
                            SalesController sysmgr = new SalesController();
                            sysmgr.PlaceOrder(employeeID, paymentType, null);
                        }
                        else
                        {
                            SalesController sysmgr = new SalesController();
                            sysmgr.PlaceOrder(employeeID, paymentType, coupon.CouponID);
                        }
                        UpdateShoppingCart();
                        CouponDiv.Visible = false;
                        SubtotalCheckoutLabel.Text = "";
                        TaxTotalLabel.Text = "";
                        TotalCheckoutLabel.Text = "";
                        cartTotalLabel.Text = "";
                        ItemSelectionList.DataBind();
                    }, "Orders", "Order Placed");
                }
            }
        }
        protected void MyListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var LinkButton1 = (LinkButton)e.Item.FindControl("LinkButton1");
                var quantityAmount = (TextBox)e.Item.FindControl("QuantityTextBox");
                if (!Request.IsAuthenticated || !User.IsInRole(SecurityRoles.Sales)) 
                {
                    LinkButton1.Visible = false;
                    quantityAmount.Visible = false;
                }
                else
                {
                    LinkButton1.Visible = true;
                    quantityAmount.Visible = true;
                }
            }
            
        }
    }
}