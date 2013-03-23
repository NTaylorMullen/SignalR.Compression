using Microsoft.AspNet.SignalR.Compression.Tests.Common.Payloads;
using Microsoft.AspNet.SignalR.Compression.Tests.Common.Utilities;
using Xunit;

namespace Microsoft.AspNet.SignalR.Compression.Server.Tests
{
    public class PayloadProviderFacts
    {
        [Fact]
        public void FindsBaseClassesWithPayloadAttribute()
        {
            var provider = TestUtilities.BuildPayloadDescriptorProvider();
            var type = typeof(Person);
            var payload = provider.GetPayload(type);

            Assert.NotNull(payload);

            TestUtilities.ValidateMembersToPayloadDescriptor(type, payload);
        }

        [Fact]
        public void PayloadDescriptorsHaveCorrectCompressionSettings()
        {
            var provider = TestUtilities.BuildPayloadDescriptorProvider();
            var payload = provider.GetPayload(typeof(Person));

            Assert.NotNull(payload);
            Assert.Equal(payload.Settings.DigitsToMaintain, -1);

            payload = provider.GetPayload(typeof(Teacher));

            Assert.NotNull(payload);
            Assert.Equal(payload.Settings.DigitsToMaintain, 3);
        }

        [Fact]
        public void DoesNotFindClassesWithoutPayloadAttribute()
        {
            var provider = TestUtilities.BuildPayloadDescriptorProvider();

            Assert.Null(provider.GetPayload(typeof(PayloadProviderFacts)));
        }

        [Fact]
        public void FindsClassesInheritingClassWithPayloadAttribute()
        {
            var provider = TestUtilities.BuildPayloadDescriptorProvider();
            var type = typeof(Parent);
            var payload = provider.GetPayload(type);

            Assert.NotNull(payload);

            TestUtilities.ValidateMembersToPayloadDescriptor(type, payload);
        }
    }
}
