using System.Web.Routing;

namespace Microsoft.AspNet.SignalR.Compression.Server.SystemWeb
{
    public static class CompressionBaseExtensions
    {
        public static void CompressPayloads(this CompressionBase compressionBase, RouteCollection routes)
        {
            routes.MapConnection<ContractEndpoint>("compression/contracts", "compression/contracts");

            compressionBase.CompressPayloads();
        }
    }
}
