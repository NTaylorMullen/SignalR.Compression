using SignalR.Compression.Server;

namespace SignalR.Compression.AspNet.Samples.Payloads
{
    [Payload]
    public class Parent : Person
    {
        public Parent Mother { get; set; }
        public Person[] Children { get; set; }
    }
}