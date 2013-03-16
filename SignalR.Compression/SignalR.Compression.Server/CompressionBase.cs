using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Web.Routing;

namespace SignalR.Compression.Server
{
    public class CompressionBase
    {
        /// <summary>
        /// Register the PayloadCompressionModule and all required dependency resolver pieces.
        /// </summary>
        public void CompressPayloads()
        {
            var resolver = GlobalHost.DependencyResolver;

            var payloadDescriptorProvider = new Lazy<ReflectedPayloadDescriptorProvider>(() => new ReflectedPayloadDescriptorProvider(resolver));
            resolver.Register(typeof(IPayloadDescriptorProvider), () => payloadDescriptorProvider.Value);

            var payloadCompressor = new Lazy<DefaultPayloadCompressor>(() => new DefaultPayloadCompressor(resolver));
            resolver.Register(typeof(IPayloadCompressor), () => payloadCompressor.Value);

            var payloadDecompressor = new Lazy<DefaultPayloadDecompressor>(() => new DefaultPayloadDecompressor(resolver));
            resolver.Register(typeof(IPayloadDecompressor), () => payloadDecompressor.Value);

            var contractGenerator = new Lazy<DefaultContractsGenerator>(() => new DefaultContractsGenerator(resolver));
            resolver.Register(typeof(IContractsGenerator), () => contractGenerator.Value);

            var parameterBinder = new Lazy<CompressableParameterResolver>(() => new CompressableParameterResolver(payloadDescriptorProvider.Value, payloadDecompressor.Value));
            resolver.Register(typeof(IParameterResolver), () => parameterBinder.Value);            

            resolver.Resolve<IHubPipeline>().AddModule(new PayloadCompressionModule(resolver.Resolve<IPayloadCompressor>(), resolver.Resolve<IPayloadDescriptorProvider>(), resolver.Resolve<IContractsGenerator>()));
        }
    }
}
