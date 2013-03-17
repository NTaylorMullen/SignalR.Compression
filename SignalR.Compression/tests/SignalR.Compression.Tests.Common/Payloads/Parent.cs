using SignalR.Compression.Server;

namespace SignalR.Compression.Tests.Common.Payloads
{    
    public class Parent : Person
    {
        public Parent Mother { get; set; }
        public Person[] Children { get; set; }
    }
}