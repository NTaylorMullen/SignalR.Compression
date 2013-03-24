<%@ Page Title="" Language="C#" MasterPageFile="~/SignalR.Compression.AspNet.Samples.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Compression.AspNet.Samples.MethodInvocation.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb">
        <li><a href="<%: ResolveUrl("~/") %>">SignalR Compression Samples</a> <span class="divider">/</span></li>
        <li class="active">Method Invocation</li>
    </ul>

    <div class="page-header">
        <h2>Method Invocation</h2>
        <p>Demonstrates compressed payloads being passed to methods on the server.</p>
    </div>
    <div class="row">
        <div class="form-vertical well span4">
            <fieldset>
                <legend>Person/Student Builder</legend>

                <div class="control-group">
                    <label class="control-label" for="firstName">First Name</label>
                    <div class="controls">
                        <input type="text" class="input-xlarge" id="firstName" value="John" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="lastName">Last Name</label>
                    <div class="controls">
                        <input type="text" class="input-xlarge" id="lastName" value="Doe" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="age">Age</label>
                    <div class="controls">
                        <input type="text" class="input-xlarge" id="age" value="38" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="gpa">GPA</label>
                    <div class="controls">
                        <input type="text" class="input-xlarge" id="gpa" value="1.337" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="debt">Debt</label>
                    <div class="controls">
                        <input type="text" class="input-xlarge" id="debt" value="36543.45" />
                    </div>
                </div>
                <div class="form-actions">
                    <button class="btn btn-primary" id="sendPerson">Send Person</button>
                    <button class="btn btn-inverse" id="sendStudent">Send Student</button>
                </div>
            </fieldset>
        </div>
        <div class="span4">
            <div class="form-vertical well">
                <fieldset>
                    <legend>Invoking (Compressed)</legend>
                    <div id="invokingCompressed">
                    </div>
                </fieldset>
            </div>
            <div class="form-vertical well">
                <fieldset>
                    <legend>Echo (Compressed)</legend>
                    <div id="echoCompressed">
                    </div>
                </fieldset>
            </div>
            <div class="form-vertical well">
                <fieldset>
                    <legend>Echo (Decompressed)</legend>
                    <div id="echoDecompressed">
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%: ResolveUrl("~/signalr/hubs") %>"></script>
    <script src="MethodInvocation.js"></script>
</asp:Content>
