using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.AspNet.SignalR.Compression.Server;
using Xunit;

namespace Microsoft.AspNet.SignalR.Compression.Tests.Common.Utilities
{
    public static class TestUtilities
    {
        public static void ValidateMembersToPayloadDescriptor(Type type, PayloadDescriptor payload)
        {
            var descriptors = payload.Data;
            var expectedMemers = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Union<MemberInfo>(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            Assert.Equal(expectedMemers.Count(), descriptors.Count());

            foreach (var descriptor in descriptors)
            {
                Assert.True(expectedMemers.Where(member => member.Name == descriptor.Name).Count() == 1);
            }
        }

        public static IDependencyResolver BuildCompressionDependencyResolver()
        {
            var resolver = new DefaultDependencyResolver();
            resolver.Compression().CompressPayloads(resolver);

            return resolver;
        }

        public static IPayloadDescriptorProvider BuildPayloadDescriptorProvider()
        {
            return BuildCompressionDependencyResolver().Resolve<IPayloadDescriptorProvider>();
        }

        public static IJsonSerializer BuildJsonSerializer()
        {
            return BuildCompressionDependencyResolver().Resolve<IJsonSerializer>();
        }

        public static IPayloadCompressor BuildPayloadCompressor()
        {
            return BuildCompressionDependencyResolver().Resolve<IPayloadCompressor>();
        }

        public static IPayloadDecompressor BuildPayloadDecompressor()
        {
            return BuildCompressionDependencyResolver().Resolve<IPayloadDecompressor>();
        }

        public static IParameterResolver BuildCompressableParameterResolver()
        {
            return BuildCompressionDependencyResolver().Resolve<IParameterResolver>();
        }
    }
}
