using Microsoft.AspNet.SignalR.Json;
using Microsoft.AspNet.SignalR.Compression.Tests.Common.Utilities;
using Xunit;

namespace Microsoft.AspNet.SignalR.Compression.Server.Tests
{
    public class PayloadDecompressorFacts
    {
        [Fact]
        public void DecompressDoesNotDecompressNonPayloads()
        {
            var Decompressor = TestUtilities.BuildPayloadDecompressor();

            foreach (var payload in TestData.GetNonCompressableDataSet())
            {
                var type = payload != null ? payload.GetType() : typeof(object);
                Assert.Equal(Decompressor.Decompress(payload, type), payload);
            }
        }

        [Fact]
        public void DecompressesComplexPayloadCorrectly()
        {
            var decompressor = TestUtilities.BuildPayloadDecompressor();
            var serializer = TestUtilities.BuildJsonSerializer();
            var compressed = TestData.GetExpectedCompressableDataSetResult();
            var decompressed = TestData.GetCompressableDataSet();

            for (var i = 0; i < compressed.Length; i++)
            {
                Assert.Equal(serializer.Stringify(decompressor.Decompress(compressed[i], decompressed[i].GetType())), serializer.Stringify(decompressed[i]));
            }
        }
    }
}
