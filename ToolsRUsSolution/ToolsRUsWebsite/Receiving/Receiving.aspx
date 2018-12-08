<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Receiving.aspx.cs" Inherits="ToolsRUsWebsite.Receiving.Receving" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Receive Orders</h1>
    <h3>Outstanding orders</h3>
<uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <asp:GridView ID="GridViewOutstandingOrders" runat="server" AutoGenerateColumns="False" BorderStyle="None" GridLines="Horizontal" CellPadding="5" CellSpacing="5" PageSize="15" RowStyle-Wrap="False" RowStyle-Height="30" HeaderStyle-Height="30">
        <Columns>
            <asp:TemplateField HeaderText="Purchase Order Number" SortExpression="PurchaseOrderNumber" HeaderStyle-Width="200px" >
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("PurchaseOrderNumber") %>' ID="PurchaseOrderNumber"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Order Date" SortExpression="OrderDate" HeaderStyle-Width="200px" >
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("OrderDate","{0:MMMM dd, yyyy}") %>' ID="OrderDate"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vendor Name" SortExpression="VendorName" HeaderStyle-Width="200px" >
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("VendorName") %>' ID="VendorName"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vendor Contact Phone" SortExpression="Phone" HeaderStyle-Width="200px" >
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Phone") %>' ID="Phone"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="75px">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkSelect" runat="server" CommandArgument='<%# Eval("PurchaseOrderID") %>' OnCommand="OutstandingOrderList_ItemCommand">
                            Select
                        </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
