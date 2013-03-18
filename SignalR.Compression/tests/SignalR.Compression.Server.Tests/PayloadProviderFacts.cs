using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using SignalR.Compression.Tests.Common.Payloads;
using Xunit;

namespace SignalR.Compression.Server.Tests
{
    public class PayloadProviderFacts
    {
        [Fact]
        public void FindsBaseClassesWithPayloadAttribute()
        {
            var provider = BuildPayloadDescriptorProvider();
            var type = typeof(Person);
            var payload = provider.GetPayload(type);

            Assert.NotNull(payload);

            ValidateMembersToPayloadDescriptor(type, payload);
        }

        [Fact]
        public void DoesNotFindClassesWithoutPayloadAttribute()
        {
            var provider = BuildPayloadDescriptorProvider();

            Assert.Null(provider.GetPayload(typeof(PayloadProviderFacts)));
        }

        [Fact]
        public void FindsClassesInheritingClassWithPayloadAttribute()
        {
            var provider = BuildPayloadDescriptorProvider();
            var type = typeof(Parent);
            var payload = provider.GetPayload(type);

            Assert.NotNull(payload);

            ValidateMembersToPayloadDescriptor(type, payload);
        }

        private void ValidateMembersToPayloadDescriptor(Type type, PayloadDescriptor payload)
        {
            var descriptors = payload.Data;
            var expectedMemers = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Union<MemberInfo>(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            Assert.Equal(expectedMemers.Count(), descriptors.Count());

            foreach (var descriptor in descriptors)
            {
                Assert.True(expectedMemers.Where(member => member.Name == descriptor.Name).Count() == 1);
            }
        }

        private IPayloadDescriptorProvider BuildPayloadDescriptorProvider()
        {
            var resolver = new DefaultDependencyResolver();
            resolver.Compression().CompressPayloads(resolver);

            return resolver.Resolve<IPayloadDescriptorProvider>();
        }
    }
}
