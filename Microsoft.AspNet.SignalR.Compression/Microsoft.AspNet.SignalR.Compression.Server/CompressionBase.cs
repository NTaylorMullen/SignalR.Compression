using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    public class CompressionBase
    {
        public void CompressPayloads()
        {
            CompressPayloads(GlobalHost.DependencyResolver);
        }
        /// <summary>
        /// Register the PayloadCompressionModule and all required dependency resolver pieces.
        /// </summary>
        public void CompressPayloads(IDependencyResolver resolver)
        {
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
