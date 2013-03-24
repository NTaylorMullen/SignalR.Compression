
namespace Microsoft.AspNet.SignalR.Compression.Server
{
    public class CompressionSettings
    {
        public static CompressionSettings Default = new CompressionSettings
        {
            RoundNumbersTo = -1
        };

        public int RoundNumbersTo { get; set; }
    }
}
