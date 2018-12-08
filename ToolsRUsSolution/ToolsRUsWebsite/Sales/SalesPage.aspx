<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesPage.aspx.cs" Inherits="ToolsRUsWebsite.Sales.SalesPage" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/Content/bootstrap-theme.css" rel="stylesheet" />
    <link href="/Content/bootstrap.css" rel="stylesheet" />

    <script>
      $(document).ready(function () {
            $('.nav-tabs a[href="<%=tabPanel%>"]').tab('show');
        });
       
    </script>

    <div class="row" style="height: 100px">
        <div class="col-sm-12">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-md-10">
            <h1>Sales</h1>
        </div>
        <div class="col-md-2">
            <br />
            <asp:Button UseSubmitBehavior="false" ID="CancelButton" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Cancel_Button" Width="160px" />
        </div>
    </div>
    <br />
    <br />

    <!-- Nav tabs -->
    <ul class="nav nav-tabs nav-justified">
        <li class="nav-item active">
            <a class="nav-link active" data-toggle="tab" href="#panel1" role="tab">Continue Shopping</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#panel2" role="tab">Cart</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" data-toggle="tab" href="#panel3" role="tab">Checkout</a>
        </li>
    </ul>
    <!-- Tab panels -->
    <div class="tab-content card">
        <!--Panel 1-->
        <div class="tab-pane fade in active" id="panel1" role="tabpanel">
            <div class="row">
                <div class="col-xs-12">
                    <h2 style="margin-top: 0">Shopping</h2>
                </div>
                <div class="col-md-3">
                    <div style="background: #e8e8ee; padding: 10px;">
                        <h3>Select A Category</h3>
                        <div class="row">
                            <div class="col-xs-8">
                                <asp:LinkButton OnClick="AllCategory_Button" ID="AllCategoriesButton" runat="server">All</asp:LinkButton>
                            </div>
                            <div class="col-xs-3">
                                <asp:Label ID="AllCategoryItemCount" Text="TODO" CssClass="label label-primary text-right" runat="server"></asp:Label>
                            </div>
                        </div>
                        <br />
                        <asp:Repeater ID="CategoryRepeater" runat="server" DataSourceID="CategoryListODS" ItemType="ToolsRUs.Data.POCOs.CategoryList">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="col-xs-8">
                                        <asp:LinkButton OnClick="Category_Button" ID="CategoryButton" CommandArgument="<%# Item.CategoryID %>" runat="server"><%# Item.Description %></asp:LinkButton>
                                    </div>
                                    <div class="col-xs-3">
                                        <asp:Label ID="CategoryItemCount" Text="<%# Item.ItemCount %>" CssClass="label label-primary text-right" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-8">
                    <div style="height: 340px">
                        <asp:Label ID="Category" Text="-1" runat="server" Visible="false"></asp:Label>
                        <asp:ListView ID="ItemSelectionList" runat="server" DataSourceID="ItemListODS" OnItemDataBound="MyListView_ItemDataBound" OnItemCommand="AddButton_OnClick">
                            <AlternatingItemTemplate>
                                <tr style="">
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("StockItemID") %>' CssClass="btn btn-primary">Add</asp:LinkButton>
                                        &nbsp&nbsp</td>
                                    <td>
                                        <asp:TextBox ID="QuantityTextBox" Width="30px" runat="server">1</asp:TextBox>&nbsp&nbsp</td>
                                    <td>
                                        <asp:Label Text='<%# string.Format("{0:C}", Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                    <td>
                                        <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                                    <td>
                                        <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" CssClass="label label-default" ID="QuantityOnHandLabel" /></td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table runat="server" style="">
                                    <tr>
                                        <td>No items in your cart</td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="">
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("StockItemID") %>' CssClass="btn btn-primary">Add</asp:LinkButton>
                                        &nbsp&nbsp</td>
                                    <td>
                                        <asp:TextBox ID="QuantityTextBox" Width="30px" runat="server">1</asp:TextBox>&nbsp&nbsp</td>
                                    <td>
                                        <asp:Label Text='<%# string.Format("{0:C}", Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                    <td>
                                        <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</td>
                                    <td>
                                        <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" CssClass="label label-default" ID="QuantityOnHandLabel" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                                <tr runat="server" style="">
                                                    <%--<th runat="server">ADD</th>
                                                    <th runat="server">PurchasePrice</th>
                                                    <th runat="server">Description</th>
                                                    <th runat="server">QuantityOnHand</th>--%>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" style="text-align: center; background-color: #FFFFFF; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000">
                                            <asp:DataPager runat="server" ID="DataPager1" PageSize="9">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                    <asp:NumericPagerField></asp:NumericPagerField>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                    <br />
                    <div class="text-right">
                        <a class="btn btn-primary active" id="cartButton" data-toggle="tab" href="#panel2" role="tab" style="width: 160px">Cart ></a>
                        <script>$('#cartButton').click(function (e) {
    $('.nav-tabs a[href="#panel2"]').tab('show');
})
                        </script>
                    </div>
                </div>
            </div>
        </div>
        <!--/.Panel 1-->









        <!--Panel 2-->
        <div class="tab-pane fade in" id="panel2" role="tabpanel">
            <h2>Cart</h2>
            <div class="row" style="font-size: 20px">
                <div class="col-xs-1"></div>
                <div class="col-xs-11 pre-scrollable" style="min-height: 340px">
                    <h3>Cart Items</h3>
                    <asp:Label ID="ShoppingCartIDLabel" runat="server" Visible="false"></asp:Label>
                    <asp:ListView ID="ShoppingCartItemListView" runat="server" DataSourceID="ShoppingCartItemListODS" OnItemCommand="CartItemUpdate_Button">
                        <AlternatingItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label Text='<%# Eval("ShoppingCartItemID") %>' Visible="false" runat="server" ID="StockItemIDLabel" /></td>

                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td style="text-align:right">
                                    <asp:Label Text='<%# string.Format("{0:C}", Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td style="text-align:right">
                                    <asp:Label runat="server" ID="shoppingCartItemTotalLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td>
                                    <asp:LinkButton ID="DeleteCartItemButton" runat="server" CommandArgument='<%# Eval("ShoppingCartItemID") %>' CssClass="glyphicon glyphicon-trash" OnClick="CartItemDelete_Button"></asp:LinkButton>
                                    &nbsp&nbsp</td>
                                <td>
                                    <asp:TextBox ID="QuantityTextBox" Width="40px" Text='<%# Eval("Quantity") %>' runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:LinkButton ID="UpdateCartItemButton" runat="server"  CssClass=" glyphicon glyphicon-refresh"></asp:LinkButton>
                                    &nbsp&nbsp</td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="">
                                <tr>
                                    <td>No Items in your cart</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label Text='<%# Eval("ShoppingCartItemID") %>' Visible="false" runat="server" ID="StockItemIDLabel" /></td>

                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td style="text-align:right">
                                    <asp:Label Text='<%# string.Format("{0:C}", Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td style="text-align:right">
                                    <asp:Label ID="shoppingCartItemTotalLabel" runat="server" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td>
                                    <asp:LinkButton ID="DeleteCartItemButton" runat="server" CommandArgument='<%# Eval("ShoppingCartItemID") %>' CssClass=" glyphicon glyphicon-trash" OnClick="CartItemDelete_Button"></asp:LinkButton>
                                    &nbsp&nbsp</td>
                                <td>
                                    <asp:TextBox ID="QuantityTextBox" Width="40px" Text='<%# Eval("Quantity") %>' runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:LinkButton ID="UpdateCartItemButton" runat="server" CssClass=" glyphicon glyphicon-refresh"></asp:LinkButton>
                                    &nbsp&nbsp</td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                            <tr runat="server" style="">
                                                <th runat="server"></th>
                                                <th runat="server"></th>
                                                <th runat="server">Price</th>
                                                <th runat="server">Total</th>
                                                <th runat="server"></th>
                                                <th runat="server">Quantity</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style=""></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                    <br />
                    <div class="row">
                        <div class="col-xs-5"></div>
                        <div class="col-xs-7">
                            <asp:Label ID="cartTotalLabel" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <a class="btn btn-primary active" id="shoppingButton" data-toggle="tab" href="#panel1" role="tab" style="width: 160px">< Continue Shopping</a>
                    <script>$('#shoppingButton').click(function (e) {
    $('.nav-tabs a[href="#panel1"]').tab('show');
})
                    </script>
                </div>
                <div class="col-xs-6 text-right">
                    <a class="btn btn-primary active" id="checkoutButton" data-toggle="tab" href="#panel3" role="tab" style="width: 160px">Checkout ></a>
                    <script>$('#checkoutButton').click(function (e) {
    $('.nav-tabs a[href="#panel3"]').tab('show');
})
                    </script>
                </div>
            </div>
        </div>
        <!--/.Panel 2-->






        <!--Panel 3-->
        <div class="tab-pane fade in" id="panel3" role="tabpanel">
            <h2>Checkout</h2>
            <div class="row" style="font-size: 20px">
                <div class="col-md-8 pre-scrollable" style="height: 340px">
                    <asp:ListView ID="CheckoutListView" runat="server" DataSourceID="ShoppingCartItemListODS">
                        <AlternatingItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td>
                                    <asp:Label ID="QuantityTextBox" Width="40px" Text='<%# Eval("Quantity") %>' runat="server"></asp:Label></td>
                                <td style="text-align:right">
                                    <asp:Label Text='<%# string.Format("{0:C}", Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td style="text-align:right">
                                    <asp:Label runat="server" ID="CheckoutItemTotalLabel" />&nbsp&nbsp&nbsp&nbsp</td>

                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="">
                                <tr>
                                    <td>No items to display</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td>
                                    <asp:Label ID="QuantityTextBox" Width="40px" Text='<%# Eval("Quantity") %>' runat="server"></asp:Label></td>
                                <td style="text-align:right">
                                    <asp:Label Text='<%# string.Format("{0:C}", Eval("PurchasePrice")) %>' runat="server" ID="PurchasePriceLabel" />&nbsp&nbsp&nbsp&nbsp</td>
                                <td style="text-align:right">
                                    <asp:Label ID="CheckoutItemTotalLabel" runat="server" />&nbsp&nbsp&nbsp&nbsp</td>

                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                                            <tr runat="server" style="">
                                                <th runat="server"></th>
                                                <th runat="server">Qty</th>
                                                <th runat="server">Price</th>
                                                <th runat="server">Total</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style=""></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                    </asp:ListView>
                </div>
                <div class="col-md-4">
                    <div class="row">
                        <div class="col-xs-4">
                            <p><b>Coupon</b></p>
                        </div>
                        <div class="col-xs-6">
                            <asp:TextBox ID="CouponTextBox" Width="150px" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-xs-2">
                            <asp:Button ID="CouponButton" runat="server" CssClass="btn btn-primary" OnClick="CouponButton_OnClick" Text="Add" />
                        </div>
                    </div>
                    <br />
                    <div style="text-align:right">
                        <div class="row">
                            <div class="col-xs-6">
                                <p><b>SubTotal</b></p>
                            </div>
                            <div class="col-xs-6">
                                <asp:Label ID="SubtotalCheckoutLabel" runat="server"></asp:Label>
                            </div>                         
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <p><b>Tax</b></p>
                            </div>
                            <div class="col-xs-6">
                                <asp:Label ID="TaxTotalLabel" runat="server"></asp:Label>
                            </div>                             
                        </div>
                        <div class="row" id="CouponDiv" runat="server" visible="false">                           
                            <div class="col-xs-6">
                                <p><b>Discount</b></p>
                            </div>
                            <div class="col-xs-6">
                                <asp:Label ID="CouponAmountLabel" runat="server" Text="0"></asp:Label>
                                <asp:Label ID="CouponId" Visible="false" Text ="" runat="server"></asp:Label>
                            </div>                           
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <p><b>Total</b></p>
                            </div>
                            <div class="col-xs-6">
                                <asp:Label ID="TotalCheckoutLabel" runat="server"></asp:Label>
                            </div>                         
                        </div>
                        <div class="row" >
                            <div class="col-xs-12">
                                <p><b>Payment Type</b></p>
                            </div>
                        </div>
                        <div class="row" >
                            <div class="col-xs-8"></div>
                            <div class="col-xs-4" style="text-align:left">
                                <asp:RadioButtonList CssClass="radio-inline"  ID="RadioButtonList1" runat="server">
                                    <asp:ListItem Selected="True" Text="Money" Value ="M"></asp:ListItem>
                                    <asp:ListItem Text="Credit" Value ="C"></asp:ListItem>
                                    <asp:ListItem Text="Debit" Value ="D"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <a class="btn btn-primary active" id="backToCartButton" data-toggle="tab" href="#panel2" role="tab" style="width: 160px">< Cart</a>
                    <script>$('#backToCartButton').click(function (e) {
    $('.nav-tabs a[href="#panel2"]').tab('show');
})
                    </script>
                </div>
                <div class="col-xs-6 text-right">
                    <asp:Button ID="PlaceOrderButton" runat="server" Text="PlaceOrder" href="#panel3" OnClick="PlaceOrder_ButtonClick" CssClass="btn btn-primary"  Width="160px" />
                </div>
            </div>
        </div>
        <!--/.Panel 3-->
    </div>



    <%--ODS--%>

    <%--panel 1--%>
    <asp:ObjectDataSource ID="CategoryListODS" runat="server" SelectMethod="List_Categories" TypeName="ToolsRUsSystem.BLL.CategoryController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ItemListODS" runat="server" SelectMethod="List_ItemsForCartSelection" TypeName="ToolsRUsSystem.BLL.StockItemController">
        <SelectParameters>
            <asp:ControlParameter ControlID="Category" PropertyName="Text" Name="categoryID" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>


    <%--panel 2--%>
    <asp:ObjectDataSource ID="ShoppingCartItemListODS" runat="server" SelectMethod="List_ItemsFromCart" TypeName="ToolsRUsSystem.BLL.ShoppingCartItemController">
        <SelectParameters>
            <asp:ControlParameter ControlID="ShoppingCartIDLabel" PropertyName="Text" Name="ShoppingCartID" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>



    <%--panel 3--%>
</asp:Content>
