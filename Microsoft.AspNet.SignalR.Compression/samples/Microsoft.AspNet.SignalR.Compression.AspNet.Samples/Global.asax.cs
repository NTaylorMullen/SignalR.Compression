using System;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Compression.Server;
using Microsoft.AspNet.SignalR.Compression.Server.SystemWeb;

namespace Microsoft.AspNet.SignalR.Compression.AspNet.Samples
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalHost.DependencyResolver.Compression().CompressPayloads(RouteTable.Routes);
            RouteTable.Routes.MapHubs();
        }
    }
}