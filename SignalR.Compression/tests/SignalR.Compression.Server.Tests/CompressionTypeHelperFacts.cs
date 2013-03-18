using System;
using SignalR.Compression.Server;
using SignalR.Compression.Tests.Common.Payloads;
using Xunit;

namespace SignalR.Compression.Server.Tests
{
    public class CompressionTypeHelperFacts
    {
        [Fact]
        public void GetCompressionTypeDecodesEnumerableDataDescriptor()
        {
            var descriptor = new DataDescriptor
            {
                Enumerable = true
            };

            Assert.Equal(CompressionTypeHelper.GetCompressionType(descriptor), CompressionTypeHelper.EnumerableTypeId);
        }

        [Fact]
        public void GetCompressionTypeDecodesNumericDataDescriptor()
        {
            var descriptor = new DataDescriptor
            {
                Type = typeof(Double)
            };

            Assert.Equal(CompressionTypeHelper.GetCompressionType(descriptor), CompressionTypeHelper.NumericTypeId);
        }

        [Fact]
        public void GetCompressionTypeDecodesDefaultDataDescriptor()
        {
            var descriptor = new DataDescriptor
            {
                Type = typeof(Person)
            };

            Assert.Equal(CompressionTypeHelper.GetCompressionType(descriptor), CompressionTypeHelper.DefaultTypeId);
        }
    }
}
