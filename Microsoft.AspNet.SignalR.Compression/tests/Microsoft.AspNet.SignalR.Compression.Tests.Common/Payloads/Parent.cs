using Microsoft.AspNet.SignalR.Compression.Server;

namespace Microsoft.AspNet.SignalR.Compression.Tests.Common.Payloads
{    
    public class Parent : Person
    {
        public Parent Mother { get; set; }
        public Person[] Children { get; set; }
    }
}