using System;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    public interface IPayloadDecompressor
    {
        /// <summary>
        /// Decompresses the payload into the expected Type only if the <see cref="expected"/> is a payload.
        /// </summary>
        /// <param name="payload">An object that may or may not be a payload</param>
        /// <param name="expected">The type to decompress the payload into</param>
        /// <returns>If it's a Payload it will return an an object of type <see cref="expected"/>, if not will return the original payload</returns>
        object Decompress(object payload, Type expected);
    }
}
