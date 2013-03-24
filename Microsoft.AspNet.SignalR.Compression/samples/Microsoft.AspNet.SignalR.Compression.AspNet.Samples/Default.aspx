<%@ Page Title="" Language="C#" MasterPageFile="~/SignalR.Compression.AspNet.Samples.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Compression.AspNet.Samples.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hero-unit">
        <h1>SignalR Compression</h1>
        <p>Compression Library for SignalR</p>
    </div>

    <div class="page-header">
        <h2>Samples</h2>
    </div>

    <!-- Samples -->
    <div class="row">
        <div class="span4">
            <h3>Method Invocation</h3>
            <p>Demonstrates compressed payloads being passed to methods on the server.</p>
            <p><a class="btn" href="MethodInvocation/">View sample &raquo;</a></p>
        </div>
        <div class="span4">
            <h3>Method Returner</h3>
            <p>Demonstrates compressed complex payloads being returned from methods on the server.</p>
            <p><a class="btn" href="MethodReturner/">View sample &raquo;</a></p>
        </div>  
    </div>

    <div class="clear"></div>
</asp:Content>
