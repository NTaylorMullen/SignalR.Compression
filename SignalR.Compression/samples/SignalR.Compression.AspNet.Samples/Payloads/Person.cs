using SignalR.Compression.Server;

namespace SignalR.Compression.AspNet.Samples.Payloads
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