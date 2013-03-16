using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Compression.Server;

namespace SignalR.Compression.AspNet.Samples.Payloads
{
    [Payload]
    public class Student : Person
    {
        public double GPA { get; set; }
        public decimal Debt { get; set; }
    }
}