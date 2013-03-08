
namespace SignalR.Compression
{
    public class CompressionSettings
    {
        public static CompressionSettings Default = new CompressionSettings
        {
            DigitsToMaintain = -1
        };

        public int DigitsToMaintain { get; set; }
    }
}
