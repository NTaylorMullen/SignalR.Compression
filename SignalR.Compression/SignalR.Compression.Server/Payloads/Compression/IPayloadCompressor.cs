
namespace SignalR.Compression.Server
{
    public interface IPayloadCompressor
    {
        object Compress(object payload);
        object Compress(object payload, CompressionSettings settings);
    }
}
