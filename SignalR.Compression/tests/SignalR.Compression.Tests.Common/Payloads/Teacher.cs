using System.Collections.Generic;
using SignalR.Compression.Server;

namespace SignalR.Compression.AspNet.Samples.Payloads
{
    [Payload]
    public class Teacher : Person
    {
        public List<Student> Students { get; set; }
    }
}