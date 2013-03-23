<%@ Page Title="" Language="C#" MasterPageFile="~/Microsoft.AspNet.SignalR.Compression.AspNet.Samples.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Compression.AspNet.Samples.MethodReturner.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb">
        <li><a href="<%: ResolveUrl("~/") %>">SignalR Compression Samples</a> <span class="divider">/</span></li>
        <li class="active">Method Returner</li>
    </ul>

    <div class="page-header">
        <h2>Method Returner</h2>
        <p>Demonstrates compressed complex payloads being returned from methods on the server.</p>
    </div>
    <div class="row">
        <div class="form-vertical well span4">
            <fieldset>
                <legend>Teacher/Parent Retriever</legend>
                <div class="form-actions">
                    <button class="btn btn-primary" id="getParent">Get Parent</button>
                    <button class="btn btn-inverse" id="getTeacher">Get Teacher</button>
                </div>
            </fieldset>
        </div>
        <div class="form-vertical well span3">
            <fieldset>
                <legend>Compressed</legend>
                <div id="resultCompressed">
                </div>
            </fieldset>
        </div>
        <div class="form-vertical well span3">
            <fieldset>
                <legend>Decompressed</legend>
                <div id="resultDecompressed">
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%: ResolveUrl("~/signalr/hubs") %>"></script>
    <script src="MethodReturner.js"></script>
</asp:Content>
