
namespace SignalR.Compression
{
    public interface IPayloadCompressor
    {
        object Compress(object payload);
        object Compress(object payload, CompressionSettings settings);
    }
}
