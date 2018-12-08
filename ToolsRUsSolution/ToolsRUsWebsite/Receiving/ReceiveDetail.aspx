<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiveDetail.aspx.cs" Inherits="ToolsRUsWebsite.Receiving.ReceiveDetail" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Receive Order Details</h1>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" /> 
    <asp:Label ID="PurchaseOrderNumber" runat="server" Text="" Font-Bold="True" Font-Size="Medium" Width="300"></asp:Label>
    <asp:Label ID="VendorName" runat="server" Text="" Font-Bold="True" Font-Size="Medium" Width="300"></asp:Label>
    <asp:Label ID="VendorPhone" runat="server" Font-Bold="True" Font-Size="Medium" Width="400px"></asp:Label>
    <asp:Button ID="ButtonReceive" runat="server" CssClass="btn btn-primary" Text="Receive" OnClick="Receive_Click" CausesValidation="False" />
    <asp:Button ID="ButtonCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="Cancel_Click" CausesValidation="False" />
   
    <asp:GridView ID="GridViewReceiveOrderDetail" runat="server" AutoGenerateColumns="False" BorderStyle="None" GridLines="Horizontal" CellPadding="5" CellSpacing="5" PageSize="15" RowStyle-Wrap="False" RowStyle-Height="30">
        <Columns>
            <asp:TemplateField HeaderText="Stock Item ID" SortExpression="StockItemID" HeaderStyle-Width="250px">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("StockItemID") %>' ID="StockItemID"></asp:Label>
                    <asp:Label runat="server" Text='<%# Bind("PurchaseOrderDetailID") %>' ID="PurchaseOrderDetailID" Visible="False"></asp:Label>
                    <asp:Label runat="server" Text='<%# Bind("VendorStockNumber") %>' ID="VendorStockNumber" Visible="False"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Stock Item Description" SortExpression="StockItemDescription" HeaderStyle-Width="300px">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("StockItemDescription") %>' ID="StockItemDescription"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity On Order" SortExpression="QuantityOnOrder" HeaderStyle-Width="150px">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("QuantityOnOrder") %>' ID="QuantityOnOrder"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity Outstanding" SortExpression="QuantityOutstanding" HeaderStyle-Width="150px">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("QuantityOutstanding") %>' ID="QuantityOutstanding"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity Received" SortExpression="QuantityReceived" HeaderStyle-Width="200px">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="QuantityReceived" Text="0" ></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity Returned" SortExpression="QuantityReturned" HeaderStyle-Width="200px">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="QuantityReturned" Text="0"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Return Reason" SortExpression="ReturnReason" HeaderStyle-Width="200px">
                <ItemTemplate>
                     <asp:RegularExpressionValidator ID="ReasonLength" runat="server" ErrorMessage="Reason must be 50 characters or less" ValidationExpression='^[\s\S]{0,50}$' ControlToValidate="ReturnReason" Display="None"></asp:RegularExpressionValidator>
                    <asp:TextBox runat="server" ID="ReturnReason"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <h3>Unordered Returns</h3>
    <asp:GridView ID="GridViewUnorderedPurchaseItem" runat="server" AutoGenerateColumns="False" BorderStyle="None" GridLines="Horizontal" CellPadding="5" CellSpacing="5" PageSize="15" RowStyle-Wrap="False" ShowFooter="True" RowStyle-Height="30" FooterStyle-Height="30" HeaderStyle-Height="30">
        <Columns>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("CartID") %>' ID="CartID"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="Description" HeaderStyle-Width="200px">
                <FooterTemplate>
                    <asp:TextBox ID="TextBoxDescription" runat="server"></asp:TextBox>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Description"></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="200px"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vendor Stock Number" SortExpression="VendorStockNumber" HeaderStyle-Width="200px">
                <FooterTemplate>
                    <asp:TextBox ID="TextBoxVendorStockNumber" runat="server"></asp:TextBox>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("VendorStockNumber") %>' ID="VendorStockNumber"></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="200px"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity" HeaderStyle-Width="200px">
                <FooterTemplate>
                    <asp:TextBox ID="TextBoxQuantity" runat="server" Text="0"></asp:TextBox>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Quantity") %>' ID="Quantity"></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="200px"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:LinkButton ID="ButtonAdd" runat="server" Text="Add" CausesValidation="True" CommandArgument="Add" OnCommand="UnorderedPurchaseItemCart_ItemCommand"></asp:LinkButton>
                    <asp:LinkButton ID="ButtonClear" runat="server" Text="Clear" CommandArgument="Clear" OnCommand="UnorderedPurchaseItemCart_ItemCommand" CausesValidation="False"></asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("CartID") %>' OnCommand="UnorderedPurchaseItemCart_ItemCommand" CausesValidation="False" ID="ButtonRemove">Remove</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <h3>Close Order</h3>
    <asp:Button ID="ForceClose" runat="server" CssClass="btn btn-primary" Text="Force Close" OnClick="ForceClose_Click" CausesValidation="False" />
    <asp:Label ID="CloseReason" runat="server" Text="Reason"></asp:Label>
    <asp:TextBox ID="ForceCloseReason" runat="server"></asp:TextBox>
</asp:Content>
