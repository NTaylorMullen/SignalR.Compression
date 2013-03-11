using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR.Compression.AspNet.Samples.Payloads;

namespace SignalR.Compression.AspNet.Samples.MethodInvocation
{
    [HubName("methodInvocationHub")]
    public class MethodInvocationHub : Hub
    {
        public Person EchoPerson(Person person)
        {
            Clients.Caller.echo(person);

            return person;
        }

        public void EchoStudent(Student student)
        {
            Clients.Caller.echo(student);
        }
    }
}