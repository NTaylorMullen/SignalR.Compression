// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using SignalR.Compression;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace SignalR.Compression
{
    public static class HubPipelineExtensions
    {        
        public static void CompressPayloads(this IHubPipeline pipeline)
        {
            if (pipeline == null)
            {
                throw new ArgumentNullException("pipeline");
            }

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

            pipeline.AddModule(new PayloadCompressionModule(resolver.Resolve<IPayloadCompressor>(), resolver.Resolve<IPayloadDescriptorProvider>()));
        }
    }
}
