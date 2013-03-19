using System.Linq;
using SignalR.Compression.Tests.Common.Payloads;
using SignalR.Compression.Tests.Common.Utilities;
using Xunit;

namespace SignalR.Compression.Server.Tests
{
    public class DataDescriptorFacts
    {
        [Fact]
        public void CanSetValuesOnObject()
        {
            var provider = TestUtilities.BuildPayloadDescriptorProvider();
            var type = typeof(Parent);
            var firstName = "Betty";
            var payload = provider.GetPayload(type);
            var descriptor = payload.Data.Where(data => data.Name == "FirstName").FirstOrDefault();
            Parent mother = new Parent();

            Assert.NotNull(descriptor);

            descriptor.SetValue(mother, firstName);

            Assert.Equal(mother.FirstName, firstName);
        }

        [Fact]
        public void CanGetValuesOnObject()
        {
            var provider = TestUtilities.BuildPayloadDescriptorProvider();
            var type = typeof(Parent);
            var payload = provider.GetPayload(type);
            var descriptor = payload.Data.Where(data => data.Name == "FirstName").FirstOrDefault();
            Parent mother = new Parent
            {
                FirstName = "Betty"
            };

            Assert.NotNull(descriptor);

            Assert.Equal(descriptor.GetValue(mother), mother.FirstName);
        }
    }
}
