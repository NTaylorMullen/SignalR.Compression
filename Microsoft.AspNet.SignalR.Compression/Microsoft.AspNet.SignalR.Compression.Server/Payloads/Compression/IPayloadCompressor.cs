
namespace Microsoft.AspNet.SignalR.Compression.Server
{
    public interface IPayloadCompressor
    {
        /// <summary>
        /// Compresses the payload with the default compression settings
        /// </summary>
        /// <param name="payload">An object that may or may not be a payload</param>
        /// <returns>If it's a Payload it will return an array representing the payload (compressed), if not will return the original payload</returns>
        object Compress(object payload);

        /// <summary>
        /// Compresses the payload with the provided compression settings
        /// </summary>
        /// <param name="payload">An object that may or may not be a payload</param>
        /// <param name="settings">Settings to determine how to compress the payload</param>
        /// <returns>If it's a Payload it will return an array representing the payload (compressed), if not will return the original payload</returns>
        object Compress(object payload, CompressionSettings settings);
    }
}
