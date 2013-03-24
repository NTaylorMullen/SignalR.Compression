using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Compression.Server;

namespace Microsoft.AspNet.SignalR.Compression.Tests.Common.Payloads
{
    [Payload(RoundNumbersTo=2)]
    public class Student : Person
    {
        public double GPA { get; set; }
        public decimal Debt { get; set; }
    }
}