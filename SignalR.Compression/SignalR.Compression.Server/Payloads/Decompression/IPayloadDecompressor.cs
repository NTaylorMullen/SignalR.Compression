using System;

namespace SignalR.Compression.Server
{
    public interface IPayloadDecompressor
    {
        object Decompress(object payload, Type expected);
    }
}
