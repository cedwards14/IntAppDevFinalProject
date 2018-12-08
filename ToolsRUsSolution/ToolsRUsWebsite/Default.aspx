<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ToolsRUsWebsite._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%--<div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>--%>

    <div class="jumbotron">
        <h1>Team Half Chris & Double Ds</h1>
        <img src="Content/Logo.gif" alt="Logo" width="25%" height="25%" />
    </div>

    <div>
        <h2>Team Members</h2>
        <h4>Chris Edwards</h4>
        <p>Rentals</p>
        <h4>Chris Lam</h4>
        <p>Purchasing</p>
        <h4>Darcie Glenn</h4>
        <p>Receiving</p>
        <h4>Daylan Law</h4>
        <p>Sales</p>
    </div>

    <div>
        <h2>Shared Components</h2>
        <p>Below is how the Shared Components were divided amongst the team.</p>
        <ul>
            <li>Created the solution - Chris Edwards</li>
             <li>Error Messages - Chris Edwards</li>
            <li>Added navigation - Daylan Law</li>
            <li>Created entities and DbContext - Darcie Glenn</li>
            <li>Common user controls - Daylan Law</li>
            <li>About Page - Chris Lam</li>
            <li>Home Page - Chris Lam</li>
         </ul>
    </div>

    <div>
        <h2>Known Bugs</h2>
        <div>
            <h4>Version 0.0 - Alpha</h4>
            <ul>
                <li>No bugs...yet</li>
            </ul>
        </div>
        <div>
            <h4>Version 1.0 - Alpha</h4>
            <h5>Purchasing</h5>
            <ul>
                <li>The Vendor Stock Items ListView does not exclude items already on the order</li>
                <li>For an unknown reason, sometimes when an order is placed and a new one is generated, an item that has already been put on order to the Re-Order Level is on the order again. This is fixed with a browser history clear.</li>            
           </ul>
        </div>
    </div>

</asp:Content>
