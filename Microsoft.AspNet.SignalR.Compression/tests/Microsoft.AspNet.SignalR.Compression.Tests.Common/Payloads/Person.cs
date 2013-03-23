using Microsoft.AspNet.SignalR.Compression.Server;

namespace Microsoft.AspNet.SignalR.Compression.Tests.Common.Payloads
{
    [Payload]
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Parent Guardian { get; set; }
    }
}