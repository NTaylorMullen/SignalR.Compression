using System;
using System.Collections.Generic;

namespace SignalR.Compression.Server
{
    public class PayloadDescriptor
    {
        public virtual long ID { get; set; }

        public virtual IEnumerable<DataDescriptor> Data { get; set; }

        public virtual Type Type { get; set; }

        public virtual CompressionSettings Settings { get; set; }
    }
}
