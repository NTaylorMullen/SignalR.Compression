using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Json;
using SignalR.Compression.Tests.Common.Payloads;
using SignalR.Compression.Tests.Common.Utilities;
using Xunit;

namespace SignalR.Compression.Server.Tests
{
    public class PayloadCompressorFacts
    {
        [Fact]
        public void CompressDoesNotCompressNonPayloads()
        {
            var compressor = TestUtilities.BuildPayloadCompressor();

            foreach (var payload in TestData.GetNonCompressableDataSet())
            {
                Assert.Equal(compressor.Compress(payload), payload);
            }
        }

        [Fact]
        public void CompressesComplexPayloadsCorrectly()
        {
            var compressor = TestUtilities.BuildPayloadCompressor();
            var serializer = TestUtilities.BuildJsonSerializer();
            var payloads = TestData.GetCompressableDataSet();
            var results = TestData.GetExpectedCompressableDataSetResult();

            for (var i = 0; i < payloads.Length; i++)
            {
                Assert.Equal(serializer.Stringify(compressor.Compress(payloads[i])), serializer.Stringify(results[i]));
            }
        }

        [Fact]
        public void CompressAbidesByCompressionSettings()
        {
            var compressor = TestUtilities.BuildPayloadCompressor();
            var serializer = TestUtilities.BuildJsonSerializer();

            // Teacher has a DigitsToMaintain compression setting of 3
            var payload = new Teacher
            {
                Students = new List<Student>(new Student[]{
                    // Student has a DigitsToMaintain compression setting of 2, should override the teacher
                    new Student
                    {
                        GPA = 2.2345
                    }
                })
            };

            var result = new object[] 
            {
                0,
                0,
                0,
                0,
                new object[]
                {
                    new object[] 
                    {
                        0,
                        0.0,
                        0,
                        2.23,
                        0,
                        0
                    }
                }
            };

            Assert.Equal(serializer.Stringify(compressor.Compress(payload)), serializer.Stringify(result));
        }
    }
}
