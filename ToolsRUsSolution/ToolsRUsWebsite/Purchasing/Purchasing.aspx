<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Purchasing.aspx.cs" Inherits="ToolsRUsWebsite.Purchasing.Purchasing" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Purchasing</h1>
	<div class="container">
		<asp:Label ID="Label8" runat="server" Text="Employee:" Style="margin-left: 650px; display: inline"></asp:Label>
		<asp:Label ID="EmployeeName" runat="server" Text="" Style="display: inline;"></asp:Label>
		<div class="row">
			<asp:DropDownList ID="VendorDDL" runat="server" DataSourceID="VendorODS" DataTextField="VendorName" DataValueField="VendorID" AppendDataBoundItems="true">
				<asp:ListItem Text="Select a Vendor..." Value="-1" />
			</asp:DropDownList>
			<asp:Button ID="FetchVendor" runat="server" Text="Fetch" OnClick="FetchVendor_Click" CssClass="btn btn-primary"/>
		</div>
		<br />
		<div class="row">
			<asp:Button ID="Update" runat="server" Text="Update" OnClick="Update_Click" CssClass="btn btn-primary" />
			<asp:Button ID="Place" runat="server" Text="Place" OnClick="Place_Click" CssClass="btn btn-primary"/>
			<asp:Button ID="Delete" runat="server" Text="Delete" OnClick="Delete_Click" CssClass="btn btn-primary"/>
			<asp:Button ID="Clear" runat="server" Text="Clear" OnClick="Clear_Click" CssClass="btn btn-primary"/>
		</div>
		<br />
		<div class="row">
			<asp:Label ID="AddressLabel" runat="server" Text="Address: "></asp:Label><asp:Label ID="Address" runat="server" Text=""></asp:Label><br />
			<asp:Label ID="CityLabel" runat="server" Text="City: "></asp:Label><asp:Label ID="City" runat="server" Text=""></asp:Label><br />
			<asp:Label ID="PostalLabel" runat="server" Text="Postal Code: "></asp:Label><asp:Label ID="PostalCode" runat="server" Text=""></asp:Label><br />
			<asp:Label ID="PhoneLabel" runat="server" Text="Phone: "></asp:Label><asp:Label ID="Phone" runat="server" Text=""></asp:Label><br />
		</div>
		<hr />
	</div>
	<div class="row">
		<uc1:MessageUserControl runat="server" ID="MessageUserControl" />
	</div>

	<%--ACTIVE ORDER GRID--%>
	<div class="container">
		<div class="row">
			<h4>Active Order</h4>
		</div>
		<div class="row">
			<asp:Label ID="POnumber" runat="server" Text="Purchase Order: "></asp:Label>
			<asp:Label ID="PO" runat="server" Text=""></asp:Label>
		</div>
		<div class="row">
			<asp:GridView ID="CurrentOrderGrid" runat="server"
				AutoGenerateColumns="False"
				CellSpacing="20"
				BorderStyle="None" 
                GridLines="Horizontal"
				AllowPaging="True" Width="957px" Height="66px"
				OnRowCommand="CurrentOrderGrid_ItemCommand">
				<Columns>
					<asp:TemplateField HeaderText="SID" SortExpression="SID">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("SID") %>' ID="SID"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Description" SortExpression="Description">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("Description") %>' ID="Description"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="QoH" SortExpression="QoH">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("QoH") %>' ID="QuantityOnHand"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="QoO" SortExpression="QoO">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("QoO") %>' ID="QuantityOnOrder"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="RoL" SortExpression="RoL">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# Eval("RoL") %>' ID="ReOrder"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="QtO" SortExpression="QtO">
						<ItemTemplate>
							<asp:TextBox runat="server" Text='<%# Bind("QtO") %>' ID="QuantityToOrder" Width="60px" Type="number" Min="0"></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Price" SortExpression="Price">
						<ItemTemplate>
							<asp:TextBox runat="server" Text='<%# Bind("Price", "{0:0.00}") %>' ID="Price" Width="60px" Type="number" Min="0" Step="0.01"></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="" SortExpression="POID">
						<ItemTemplate>
							<asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn" CommandArgument='<%# Eval("PurchaseOrderDetailID") %>'>
								<span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;</span></asp:LinkButton>
							<asp:Label runat="server" Text='<%# Eval("PurchaseOrderDetailID") %>' Visible="false" ID="POID"></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<p>No Data Found.</p>
				</EmptyDataTemplate>
				<%--<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>
				<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></HeaderStyle>--%>
				<PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>
				<%--<RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>--%>
			</asp:GridView>
		</div>
		<div class="row">
			<asp:Label ID="SubtotalLabel" runat="server" Text="Subtotal: $"></asp:Label>
			<asp:Label ID="Subtotal" runat="server" Text="0.00"></asp:Label><br />
			<asp:Label ID="GSTLabel" runat="server" Text="GST: $"></asp:Label>
			<asp:Label ID="GST" runat="server" Text="0.00"></asp:Label><br />
			<asp:Label ID="TotalLabel" runat="server" Text="Total: $"></asp:Label>
			<asp:Label ID="Total" runat="server" Text="0.00"></asp:Label>
		</div>
	</div>
	<hr />


	<%--Vendor Stock View--%>

	<div class="container">
		<div class="row">
			<h4>Vendor Stock Items</h4>
		</div>
		<div class="row">
			<asp:ListView ID="VendorStockListView" runat="server" DataSourceID="VendorStockItemsList" CommandName="AddtoOrder" OnItemCommand="VendorStockListView_ItemCommand" >
				<AlternatingItemTemplate>
					<tr style="background-color: #FFF8DC;">
						<td>
							<asp:LinkButton ID="AddtoOrder" runat="server"
								CssClass="btn" CommandArgument='<%# Eval("SID") %>'>
                            <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
							</asp:LinkButton>
						</td>
						<td>
							<asp:Label Text='<%# Eval("SID") %>' runat="server" ID="SIDLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("QoH") %>' runat="server" ID="QoHLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("QoO") %>' runat="server" ID="QoOLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("RoL") %>' runat="server" ID="RoLLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("Buffer") %>' runat="server" ID="BufferLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("Price","{0:0.00}") %>' runat="server" ID="PriceLabel" /></td>
					</tr>
				</AlternatingItemTemplate>
				<EmptyDataTemplate>
					<table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
						<tr>
							<td>No data was returned.</td>
						</tr>
					</table>
				</EmptyDataTemplate>
				<ItemTemplate>
					<tr style="background-color: #DCDCDC; color: #000000;">
						<td>
							<asp:LinkButton ID="AddtoOrder" runat="server"
								CssClass="btn" CommandArgument='<%# Eval("SID") %>' CommandName="AddtoOrder">
                            <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
							</asp:LinkButton>
						</td>
						<td>
							<asp:Label Text='<%# Eval("SID") %>' runat="server" ID="SIDLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("QoH") %>' runat="server" ID="QoHLabel"/></td>
						<td>
							<asp:Label Text='<%# Eval("QoO") %>' runat="server" ID="QoOLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("RoL") %>' runat="server" ID="RoLLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("Buffer") %>' runat="server" ID="BufferLabel" /></td>
						<td>
							<asp:Label Text='<%# Eval("Price","{0:0.00}") %>' runat="server" ID="PriceLabel" /></td>
					</tr>
				</ItemTemplate>
				<LayoutTemplate>
					<table runat="server">
						<tr runat="server">
							<td runat="server">
								<table runat="server" cellspacing="20" cellpadding="10" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
									<tr runat="server" style="background-color: #DCDCDC; color: #000000;">
										<th runat="server">Add</th>
										<th runat="server">SID</th>
										<th runat="server">Description</th>
										<th runat="server">QoH</th>
										<th runat="server">QoO</th>
										<th runat="server">RoL</th>
										<th runat="server">Buffer</th>
										<th runat="server">Price</th>
									</tr>
									<tr runat="server" id="itemPlaceholder"></tr>
								</table>
							</td>
						</tr>
						<tr runat="server">
							<td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
								<asp:DataPager runat="server" ID="DataPager1">
									<Fields>
										<asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"></asp:NextPreviousPagerField>
									</Fields>
								</asp:DataPager>
							</td>
						</tr>
					</table>
				</LayoutTemplate>
			</asp:ListView>
		</div>
	</div>



	<%--ODS HERE--%>
	<asp:ObjectDataSource ID="VendorODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Vendor_List" TypeName="ToolsRUsSystem.BLL.VendorController"></asp:ObjectDataSource>

	<asp:ObjectDataSource ID="VendorStockItemsList" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_VendorStockItems" TypeName="ToolsRUsSystem.BLL.VendorStockItemController">
		<SelectParameters>
			<asp:ControlParameter ControlID="VendorDDL" PropertyName="SelectedValue" DefaultValue="0" Name="vendorid" Type="Int32"></asp:ControlParameter>
		</SelectParameters>
	</asp:ObjectDataSource>

	<asp:ObjectDataSource ID="ActiveOrderGridODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_PurchaseOrder_Get" TypeName="ToolsRUsSystem.BLL.PurchaseOrderDetailController">
		<SelectParameters>
			<asp:ControlParameter ControlID="VendorDDL" PropertyName="SelectedValue" DefaultValue="0" Name="vendorid" Type="Int32"></asp:ControlParameter>
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Content>
