using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Json;

namespace SignalR.Compression.Server
{
    public class ContractEndpoint : PersistentConnection
    {
        private IContractsGenerator _contractGenerator;

        public override void Initialize(IDependencyResolver resolver, HostContext context)
        {
            _contractGenerator = resolver.Resolve<IContractsGenerator>();

            base.Initialize(resolver, context);
        }
        
        private Task ProcessJsonpRequest(HostContext context, object payload)
        {
            context.Response.ContentType = JsonUtility.JavaScriptMimeType;
            var data = JsonUtility.CreateJsonpCallback(context.Request.QueryString["callback"], JsonSerializer.Stringify(payload));

            return context.Response.End(data);
        }

        public override Task ProcessRequest(HostContext context)
        {            
            var response = new Dictionary<string, object>();

            response["Contracts"] = _contractGenerator != null ? _contractGenerator.GenerateContracts() : null;

            if (!String.IsNullOrEmpty(context.Request.QueryString["callback"]))
            {
                return ProcessJsonpRequest(context, response);
            }

            context.Response.ContentType = JsonUtility.JsonMimeType;
            return context.Response.End(JsonSerializer.Stringify(response));
        }
    }
}
