using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR.Compression.Tests.Common.Payloads;

namespace SignalR.Compression.AspNet.Samples.MethodInvocation
{
    [HubName("methodReturnerHub")]
    public class MethodReturnerHub : Hub
    {
        public Parent GetParent()
        {
            return new Parent
            {
                Mother = new Parent
                {
                    FirstName = "Mom",
                    Age = 62
                    // TODO: Allow for semi-recursive loops, aka adding the top level parent as a Child of "Mom"
                },
                FirstName = "John",
                LastName = "Doe",
                Age = 36,
                Children = new Person[]{
                    new Person
                    {
                        FirstName="Daughter",
                        LastName="Doe",
                        Age=9
                    },
                    new Person
                    {
                        FirstName="Johny",
                        LastName="Doe",
                        Age=7
                    }
                }
            };
        }

        public Teacher GetTeacher()
        {
            return new Teacher
            {
                FirstName = "Teacher",
                LastName = "Dude",
                Age = 29,
                Students = new List<Student>(new Student[]{
                    new Student
                    {
                        FirstName="Numero",
                        LastName="Uno",
                        Age = 10
                    },
                    new Student
                    {
                        FirstName="Numero",
                        LastName="Dos",
                        Age = 10
                    }
                })
            };
        }
    }
}