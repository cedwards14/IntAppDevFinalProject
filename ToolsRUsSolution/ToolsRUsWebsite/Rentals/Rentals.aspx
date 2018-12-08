<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rentals.aspx.cs" Inherits="ToolsRUsWebsite.Rentals.Rentals" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="row" style="margin-top:15px;">
     <h1 style="display:inline; margin-left:15px;  margin-top:10px;">Rentals</h1>  
                   <%-- Employee info display --%>
    <asp:Label ID="Label8" runat="server" Text="Employee:" style="margin-left:650px; display:inline"></asp:Label>
     <asp:Label ID="EmployeeName" runat="server" Text="" style="display:inline;"></asp:Label>
       </div>
     <div class="row" style="margin-top:10px;"> <%-- error handling--%>
       <div class="col-sm-12">
           <uc1:MessageUserControl runat="server" id="MessageUserControl" />
       </div>
   </div>
    <div class="row" >   <%--Phone number search--%>
        <div class="col-md-4" style="display:inline-block; padding-left:0;">
            <asp:Label ID="Label1" runat="server" Text="Phone Number" Font-Bold="true"></asp:Label>
           <%-- <asp:TextBox ID="PhoneNumber" runat="server"></asp:TextBox>--%>
            <asp:TextBox ID="AreaCode" runat="server" Width="40px" MaxLength="3" placeholder="780"></asp:TextBox>
            <asp:TextBox ID="Phone1" runat="server" Width="50px" MaxLength="3" placeholder="111"></asp:TextBox>
            <asp:TextBox ID="Phone2" runat="server" Width="50px" MaxLength="4" placeholder="1111"></asp:TextBox>
            <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" CssClass="btn btn-primary" UseSubmitBehavior="False" />
        </div>
        <div class="col-md-8" style="display:inline-block">    <%-- Customer GridView --%>
            <asp:GridView ID="CustomerGV" runat="server" AutoGenerateColumns="False" CssClass="table-hover table" style="width:350px; border-color:#fff"
             >
                <Columns>
                     <asp:TemplateField HeaderText="cid" Visible="false"  >
                        <ItemTemplate>
                            <asp:Label ID="CID" runat="server" Text='<%# Eval("Id") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name"  >
                        <ItemTemplate>
                            <asp:Label ID="Name" runat="server" Text='<%# Eval("Name") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address" >
                        <ItemTemplate>
                            <asp:Label ID="Address" runat="server" Text='<%# Eval("Address") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:LinkButton ID="Fetch" runat="server"  CommandArgument='<%# Eval("Id") %>' OnCommand="Fetch_Command">Add</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </div>
    <div class="row" style="margin-top:25px;">
        <div class="col-sml-6" style="margin-left:15px;"> <%-- Customer info display --%>

            <asp:Label ID="customerID" runat="server" Text="" Visible="false" ></asp:Label>
            <asp:Label ID="Label5" runat="server" Text="Name: "  style="display:inline-block;" Font-Bold="True"></asp:Label>
            <asp:Label ID="customername" runat="server" Text=""   style="display:inline;"></asp:Label> <br />
            <asp:Label ID="Label6" runat="server" Text="Address: "  style="display:inline-block;" Font-Bold="True"></asp:Label>
            <asp:Label ID="address" runat="server" Text=""   style="display:inline;"></asp:Label><br />
            <asp:Label ID="Label7" runat="server" Text="City: "  style="display:inline-block;" Font-Bold="True"></asp:Label>
            <asp:Label ID="city" runat="server" Text=""   style="display:inline;"></asp:Label><br />
        </div>
    </div>
    <div class="row" style="margin-top:15px;">
        <div class="col-sm-6"><%-- Credit Card --%>
        <asp:Label ID="Label2" runat="server" Text="Credit Card:" Font-Bold="true"></asp:Label>
        <asp:TextBox ID="creditcard" runat="server" MaxLength="16"  onkeydown = "return  (!((event.keyCode>=65 && event.keyCode <= 95) || event.keyCode >= 106 || (event.keyCode >= 48 && event.keyCode <= 57 && isNaN(event.key))) && event.keyCode!=32);"></asp:TextBox>
       </div>
           <div class="col-sm-6" ><%-- Coupon --%>
            <asp:Label ID="Label3" runat="server" Text="Coupon" Font-Bold="true"></asp:Label>
               <asp:TextBox ID="coupon" runat="server"></asp:TextBox>
               <asp:Button ID="checkbutton" runat="server" Text="Check" CssClass="btn btn-primary" OnClick="checkbutton_Click" />
               <asp:Button ID="cancelbutton" runat="server" Text="Cancel"  CssClass="btn btn-primary" OnClick="cancelbutton_Click" />
               </div>
    </div>
     <div class="row" style="margin-top:30px;">
         <div class="col-md-6"> <%-- Available Rental List View --%>

             <asp:ListView ID="AvailableRentalEquipment" runat="server" DataSourceID="AvailableEquipmentODS"  OnItemCommand="AvailableRentalEquipment_ItemCommand" >
                 <AlternatingItemTemplate>
                     <tr style="background-color: #FFFFFF; color: #284775;">
                         <td>
                             <asp:Label Text='<%# Eval("ID") %>' runat="server" ID="IDLabel" /></td>
                         <td>
                             <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                         <td>
                             <asp:Label Text='<%# Eval("Serial") %>' runat="server" ID="SerialLabel" /></td>
                         <td align="right">
                             <asp:Label Text='<%# string.Format("${0:#,#.00}" ,Eval("Rate")) %>' runat="server" ID="RateLabel" /></td>
                         <td>
                             <asp:LinkButton ID="AddToRental" runat="server" CssClass="btn" CommandArgument='<%# Eval("ID") %>'>Add</asp:LinkButton>
                         </td>
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
                     <tr style="background-color: #FFFFFF; color: #333333;">
                         <td>
                             <asp:Label Text='<%# Eval("ID") %>' runat="server" ID="IDLabel" /></td>
                         <td>
                             <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" Width="310px" /></td>
                         <td>
                             <asp:Label Text='<%# Eval("Serial") %>' runat="server" ID="SerialLabel" Width="150px" /></td>
                         <td align="right">
                             <asp:Label Text='<%# string.Format("${0:#,#.00}" ,Eval("Rate")) %>' runat="server" ID="RateLabel" Width="75px" /></td>
                         <td>
                             <asp:LinkButton ID="AddToRental" runat="server" CssClass="btn" CommandArgument='<%# Eval("ID") %>'>Add</asp:LinkButton>
                         </td>
                     </tr>
                 </ItemTemplate>
                 <LayoutTemplate>
                     <table runat="server">
                         <tr runat="server">
                             <td runat="server">
                                 <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                     <tr runat="server" style="background-color: #FFFFFF; color: #333333;">
                                         <th runat="server">ID</th>
                                         <th runat="server">Description</th>
                                         <th runat="server">Serial #</th>
                                         <th runat="server">Rate</th>
                                     </tr>
                                     <tr runat="server" id="itemPlaceholder"></tr>
                                 </table>
                             </td>
                         </tr>
                         <tr runat="server">
                             <td runat="server" style="text-align: center; background-color: #ffffff; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000">
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
         
         <div class="col-md-6"> <%-- Rental Grid View --%>
             <asp:GridView ID="RentalGV" runat="server" AutoGenerateColumns="false" style="margin-left:100px;" CssClass="table-hover table" AllowPaging="True" PageSize="5" AllowCustomPaging="False" PageIndex="10" onPageIndexChanging="RentalGV_PageIndexChanging"  >
                 <Columns>
                     <asp:TemplateField HeaderText="DetailID" Visible="false" >
                       <ItemTemplate>
                           <asp:Label ID="DetailID" runat="server" Text='<%# Eval("DetailID") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="ID" >
                       <ItemTemplate>
                           <asp:Label ID="EquipmentID" runat="server" Text='<%# Eval("EquipmentID") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Description" >
                            <ItemTemplate>
                                <asp:Label ID="Description" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Serial#"  >
                            <ItemTemplate>
                                <asp:Label ID="SerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Rate" ItemStyle-HorizontalAlign="Right" >
                            <ItemTemplate>
                                  <asp:Label ID="Rate" runat="server" Text='<%# string.Format("${0:#,#.00}", Eval("Rate")) %>' ItemStyle-HorizontalAlign="Right" ></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Out">
                            <ItemTemplate>
                               <asp:Label ID="ConditionOut" runat="server" Text='<%# Eval("ConditionOut") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                 <asp:LinkButton ID="CurrentRentalRemove" runat="server" CommandArgument='<%# Eval("EquipmentID") + ";" + Eval("DetailID") %>' OnCommand="CurrentRentalRemove_Command">Remove</asp:LinkButton>
                       </ItemTemplate>
                     </asp:TemplateField>
                 </Columns>
             </asp:GridView>
           
         </div>



    </div>

                                  <%-- ODS--%>
<%-- Available Equipment--%><asp:ObjectDataSource ID="AvailableEquipmentODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="RentalEquipment_Available" TypeName="ToolsRUsSystem.BLL.RentalEquipmentController"></asp:ObjectDataSource>


</asp:Content>
