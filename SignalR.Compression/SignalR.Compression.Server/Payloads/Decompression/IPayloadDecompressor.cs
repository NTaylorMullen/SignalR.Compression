using System;

namespace SignalR.Compression
{
    public interface IPayloadDecompressor
    {
        object Decompress(object payload, Type expected);
    }
}
