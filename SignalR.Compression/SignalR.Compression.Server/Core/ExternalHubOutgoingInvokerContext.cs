using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR.Compression.Server
{
    public class ExternalHubOutgoingInvokerContext : IHubOutgoingInvokerContext
    {
        public ExternalHubOutgoingInvokerContext(IConnection connection, string signal, ClientHubInvocation invocation, IList<string> excludedSignals)
        {
            Connection = connection;
            Signal = signal;
            Invocation = invocation;
            ExcludedSignals = excludedSignals;
        }

        public IConnection Connection
        {
            get;
            private set;
        }

        public ClientHubInvocation Invocation
        {
            get;
            set;
        }

        public string Signal
        {
            get;
            private set;
        }

        public IList<string> ExcludedSignals
        {
            get;
            private set;
        }
    }
}
