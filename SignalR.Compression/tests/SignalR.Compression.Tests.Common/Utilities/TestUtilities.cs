using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using SignalR.Compression.Server;
using Xunit;

namespace SignalR.Compression.Tests.Common.Utilities
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

        public static IPayloadDescriptorProvider BuildPayloadDescriptorProvider()
        {
            var resolver = new DefaultDependencyResolver();
            resolver.Compression().CompressPayloads(resolver);

            return resolver.Resolve<IPayloadDescriptorProvider>();
        }
    }
}
