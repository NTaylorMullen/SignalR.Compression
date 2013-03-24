using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Compression.Server;

namespace Microsoft.AspNet.SignalR.Compression.Tests.Common.Payloads
{
    [Payload(RoundNumbersTo = 3)]
    public class Teacher : Person
    {
        public List<Student> Students { get; set; }
    }
}