 <%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ToolsRUsWebsite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div>
        <img src="Content/Logo.gif" alt="Team Half Chris & Double D"  class="center-block"/>
    </div>
    <div class="container">
        <h3>Team Members</h3>
        <div class="row">
            <div class="col-md-3 text-center">
                <h4>Chris Edwards</h4>
                <img src="https://avatars3.githubusercontent.com/u/35229175?s=460&v=4"
                    alt="Chris Edwards" width="100px" height="100px" />
            </div>
            <div class="col-md-3 text-center">
                <h4>Chris Lam</h4>
                <img src="https://avatars1.githubusercontent.com/u/23494110?s=460&v=4"
                    alt="Chris Lam" width="100px" height="100px" />
            </div>
            <div class="col-md-3 text-center">
                <h4>Darcie Glenn</h4>
                <img src="https://avatars1.githubusercontent.com/u/35244607?s=60&v=4"
                    alt="Darcie Glenn" width="100px" height="100px" />
            </div>
            <div class="col-md-3 text-center">
                <h4>Daylan Law</h4>
                <img src="https://avatars1.githubusercontent.com/u/19260870?s=60&v=4"
                    alt="Daylan Law" width="100px" height="100px" />
            </div>
        </div>
    </div>
    <br />
    <div class="container">
        <div>
            <h3>Security Roles</h3>
            <p>Default Roles:<br />Registered User: No access to admin page <br/>Staff: No access to admin page<br />Website Admin: Access to admin page <br />Purchasing: Access to the Purchasing page <br />Rentals: Access to the Rental page <br />Sales: Access to the Sales page <br />Recieving: Access to the Receiving page</p>
            <p>Default password for all accounts: Pa$$word1</p>
        </div>
    </div>
    <div class="container">
        <div>
            <h3>Connection String Values</h3>
            <p>DefaultConnection: eTools<br />ToolsRUsDB: eTools</p>
        </div>
    </div>
</asp:Content>
