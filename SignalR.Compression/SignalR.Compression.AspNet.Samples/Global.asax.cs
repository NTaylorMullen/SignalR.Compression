using System;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using SignalR.Compression.Server;
using SignalR.Compression.Server.SystemWeb;

namespace SignalR.Compression.AspNet.Samples
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