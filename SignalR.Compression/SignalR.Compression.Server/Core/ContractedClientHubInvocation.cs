using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace SignalR.Compression.Server
{
    internal class ContractedClientHubInvocation : ClientHubInvocation
    {
        public ContractedClientHubInvocation(ClientHubInvocation invocation)
        {
            Target = invocation.Target;
            Hub = invocation.Hub;
            Method = invocation.Method;
            Args = invocation.Args;
            State = invocation.State;
        }

        /// <summary>
        /// A list of contract Ids, it's a 1-to-1 map to args
        /// </summary>
        [JsonProperty("C")]
        public string[] ContractIds { get; set; }
    }
}
