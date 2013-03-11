using System.Web.Routing;

namespace SignalR.Compression.Server.SystemWeb
{
    public static class CompressionBaseExtensions
    {
        public void CompressPayloads(this CompressionBase compressionBase, RouteCollection routes)
        {
            routes.MapConnection<ContractEndpoint>("contracts", "contracts");

            compressionBase.CompressPayloads();
        }
    }
}
