using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Compression.Tests.Common.Payloads;

namespace Microsoft.AspNet.SignalR.Compression.AspNet.Samples.MethodInvocation
{
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