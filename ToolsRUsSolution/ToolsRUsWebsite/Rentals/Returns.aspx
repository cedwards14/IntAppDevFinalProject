<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Returns.aspx.cs" Inherits="ToolsRUsWebsite.Rentals.Returns" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row" style="margin-top:15px;">
     <h1 style="display:inline; margin-left:15px;  margin-top:10px;">Returns</h1>  
                   <%-- Employee info display --%>
    <asp:Label ID="Label8" runat="server" Text="Employee:" style="margin-left:650px; display:inline"></asp:Label>
     <asp:Label ID="EmployeeName" runat="server" Text="" style="display:inline;"></asp:Label>
       </div>
     <div class="row"> <%-- error handling--%>
       <div class="col-sm-12">
           <uc1:MessageUserControl runat="server" id="MessageUserControl" />
       </div>
      </div>
   <div class="row">
       <%-- Search with Rental--%>
       <div class="col-md-3">
       <div class="row">
            <asp:LinkButton ID="RentalSearchLB" runat="server" OnClick="RentalSearchLB_Click">Return</asp:LinkButton>
             <asp:TextBox ID="RentalInfo" runat="server"></asp:TextBox>
       </div>  <%-- Search with Phone Number--%>
      <div class="row" style="margin-top:15px;">
               <asp:LinkButton ID="PhoneSearchLB" runat="server" OnClick="PhoneSearchLB_Click">Phone</asp:LinkButton>
           <asp:TextBox ID="AreaCode" runat="server" Width="40px" MaxLength="3" placeholder="780"></asp:TextBox>
            <asp:TextBox ID="Phone1" runat="server" Width="50px" MaxLength="3" placeholder="111"></asp:TextBox>
            <asp:TextBox ID="Phone2" runat="server" Width="50px" MaxLength="4" placeholder="1111"></asp:TextBox>
      </div>
       </div>
     <div class="col-md-9">
           <%-- Customer Gridview--%>
       <asp:GridView ID="customerGV" runat="server" AutoGenerateColumns="false" CssClass="table-hover table" style="width:360px;">
           <Columns>
                <asp:TemplateField HeaderText="RentalID" Visible="False">
                   <ItemTemplate>
                       <asp:Label ID="rentalid" runat="server" Text='<%# Eval("rentalid") %>' ></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Name" >
                   <ItemTemplate>
                       <asp:Label ID="CustomerName" runat="server" Text='<%# Eval("Name") %>'  ></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                   <ItemTemplate>
                       <asp:Label ID="Address" runat="server" Text='<%# Eval("Address") %>' ></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rental Date">
                   <ItemTemplate>
                       <asp:Label ID="RentalDate" runat="server" Text='<%#  string.Format("{0: MMM dd yyyy}", Eval("RentalDate")) %>' ></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                   <ItemTemplate>
                       <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("rentalid") %>' OnCommand="PullRental_Command" >Select</asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>
           </Columns>
       </asp:GridView>
   </div>
       </div>
   <div class="row" style="margin-top:15px;">
         <%-- Display Customer--%>
       <asp:Label ID="Label1" runat="server" Text="Name: "  Font-Bold="true"></asp:Label>
          <asp:Label ID="Name" runat="server" Text="" ></asp:Label><br />
       <asp:Label ID="Label2" runat="server" Text="Address: "  Font-Bold="true"></asp:Label>
         <asp:Label ID="Address" runat="server" Text="" ></asp:Label><br />
       <asp:Label ID="Label3" runat="server" Text="City: " Font-Bold="true"></asp:Label>
         <asp:Label ID="City" runat="server" Text="" ></asp:Label> <br />
   </div>
   <div class="row" style="margin-top:15px;">
         <%-- Display payment info--%>
       <asp:Label ID="creditcard" runat="server" Text="CreditCard:" Font-Bold="true"></asp:Label>
       <asp:TextBox ID="creditcardTB" runat="server" Enabled="False"></asp:TextBox>
       <asp:RadioButtonList ID="paymentMethod" runat="server" RepeatDirection="Horizontal" style="display:inline; margin-left:20px;" RepeatLayout="Flow" CellPadding="-1" CellSpacing="-1">
           <asp:ListItem class="radio-inline" Text="Credit Card" value="C"/> 
           <asp:ListItem class="radio-inline" Text="Cash" value="M"/>
           <asp:ListItem class="radio-inline" Text="Debit Card" value="D"/>
       </asp:RadioButtonList>
   </div>
    <div class="row" style="display:inline-block; margin-top:15px">
        <asp:Label ID="Label4" runat="server" Text="Date Out: " Font-Bold="true" ></asp:Label>
        <asp:Label ID="Date" runat="server" Text="" style="margin-right:20px;"></asp:Label>
        <asp:Label ID="rDays" runat="server" Text="Days" Font-Bold="true" ></asp:Label>
        <asp:TextBox ID="RentalDays" runat="server" style="margin-right:20px;" Type="number" Min="0.5" Step="0.5"></asp:TextBox>
        <asp:Button ID="Return" runat="server" Text="Return" OnClick="Return_Click" CssClass="btn btn-primary" style="margin-right:20px;"   />
        <asp:Button enabled="false" ID="Pay" runat="server" Text="Pay" OnClick="Pay_Click" CssClass="btn btn-primary"  />
    </div>
   <div class="row">
         <%-- Display Rental--%>
       <asp:GridView ID="ReturnGV" runat="server" AutoGenerateColumns="false" CssClass="table-hover table" style="margin-top:15px;">
           <Columns>
               <asp:TemplateField HeaderText="ID">
                   <ItemTemplate>
                       <asp:Label ID="EquipmentID" runat="server" Text='<%# Eval("EquipmentID") %>'></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                       <asp:TemplateField HeaderText="Description">
                   <ItemTemplate>
                       <asp:Label ID="Description" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                       <asp:TemplateField HeaderText="Serial#">
                   <ItemTemplate>
                       <asp:Label ID="SerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>'></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate">
                   <ItemTemplate>
                       <asp:Label ID="ShownDailyRate" runat="server" Text='<%# string.Format("{0:C}" ,Eval("DailyRate")) %>'></asp:Label>
                   </ItemTemplate>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Rate" Visible="false">
                   <ItemTemplate>
                       <asp:Label ID="DailyRate" runat="server" Text='<%# Eval("DailyRate") %>'></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                       <asp:TemplateField HeaderText="Out">
                   <ItemTemplate>
                       <asp:Label ID="ConditionOut" runat="server" Text='<%# Eval("ConditionOut") %>'></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
                       <asp:TemplateField HeaderText="In">
                   <ItemTemplate>
                       <asp:TextBox ID="ConditionIn" runat="server" Text='<%# Eval("ConditionIn") %>'></asp:TextBox>
                   </ItemTemplate>
               </asp:TemplateField>
                       <asp:TemplateField HeaderText="Comment" >
                   <ItemTemplate>
                       <asp:TextBox ID="Comment" runat="server" Text='<%# Eval("Comment") %>'></asp:TextBox>
                   </ItemTemplate>
               </asp:TemplateField>
           </Columns>
       </asp:GridView>
   </div>
   <div class="row">
         <%-- Display Money--%>
          <asp:Label ID="subtotalLabel" runat="server" Text="Subtotal: " Font-Bold="true"></asp:Label>
          <asp:Label ID="subtotal" runat="server" Text=" "></asp:Label>
        <asp:Label ID="discountLabel" runat="server" Text="Discount: " Font-Bold="true" style="margin-left:15px;"></asp:Label>
          <asp:Label ID="discount" runat="server" Text=""></asp:Label>
              <asp:Label ID="gstLabel" runat="server" Text="GST: " Font-Bold="true" style="margin-left:15px;"></asp:Label>
          <asp:Label ID="gst" runat="server" Text=""></asp:Label>
       <asp:Label ID="totalLabel" runat="server" Text="Total: " Font-Bold="true" style="margin-left:15px;"></asp:Label>
          <asp:Label ID="total" runat="server" Text=""></asp:Label>
   </div>
</asp:Content>
