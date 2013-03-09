using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.SignalR;
using SignalR.Compression.Server;

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