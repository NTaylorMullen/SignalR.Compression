using System.Collections.Generic;
using SignalR.Compression.Server;

namespace SignalR.Compression.Tests.Common.Payloads
{
    [Payload(DigitsToMaintain = 3)]
    public class Teacher : Person
    {
        public List<Student> Students { get; set; }
    }
}