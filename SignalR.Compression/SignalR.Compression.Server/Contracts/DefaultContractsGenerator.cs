using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json.Linq;

namespace SignalR.Compression.Server
{
    public class DefaultContractsGenerator : IContractsGenerator
    {
        private Lazy<JRaw> _generatedContracts;
        private IJsonSerializer _serializer;
        private IPayloadDescriptorProvider _payloadProvider;
        private IMethodDescriptorProvider _methodProvider;
        private IHubDescriptorProvider _hubProvider;

        public DefaultContractsGenerator(IDependencyResolver resolver)
            : this(resolver.Resolve<IJsonSerializer>(),
                   resolver.Resolve<IPayloadDescriptorProvider>(),
                   resolver.Resolve<IMethodDescriptorProvider>(),
                   resolver.Resolve<IHubDescriptorProvider>())
        {
        }

        public DefaultContractsGenerator(IJsonSerializer serializer, IPayloadDescriptorProvider payloadProvider, IMethodDescriptorProvider methodProvider, IHubDescriptorProvider hubProvider)
        {
            _serializer = serializer;
            _payloadProvider = payloadProvider;
            _methodProvider = methodProvider;
            _hubProvider = hubProvider;
            _generatedContracts = new Lazy<JRaw>(() => new JRaw(_serializer.Stringify(CreateContractsCache(_serializer, _payloadProvider, _methodProvider, _hubProvider))));
        }

        private static object CreateMethodReturnContracts(IPayloadDescriptorProvider payloadProvider, IMethodDescriptorProvider methodProvider, IHubDescriptorProvider hubProvider)
        {
            return hubProvider.GetHubs()
                                .Select(hub => methodProvider.GetMethods(hub)
                                .Where(methodDescriptor => payloadProvider.HasPayload(methodDescriptor.ReturnType)))
                                .Where(methodList => methodList.Count() > 0)
                                .ToDictionary(methodList => methodList.First().Hub.Name,
                                                methodList => methodList
                                                .Select(methodDescriptor =>
                                                    {
                                                        PayloadDescriptor payloadDescriptor = payloadProvider.GetPayload(methodDescriptor.ReturnType);
                                                        bool enumerable = false;

                                                        // If payloadDescriptor is null then the return type has a payload within it
                                                        if (payloadDescriptor == null)
                                                        {
                                                            payloadDescriptor = payloadProvider.GetPayload(methodDescriptor.ReturnType.GetEnumerableType());
                                                            enumerable = true;
                                                        }

                                                        return new object[]{
                                                            methodDescriptor.Name,

                                                            new object[]{
                                                                payloadDescriptor.ID,
                                                                enumerable
                                                            }
                                                        };
                                                    }).ToDictionary(methodNameToID => methodNameToID[0],
                                                                                methodNameToID => methodNameToID[1]));
        }

        private static object CreateMethodInvokerContracts(IPayloadDescriptorProvider payloadProvider, IMethodDescriptorProvider methodProvider, IHubDescriptorProvider hubProvider)
        {
            return hubProvider.GetHubs()
                                .Select(hub => methodProvider.GetMethods(hub)
                                .Where(methodDescriptor => HasPayloadArgument(methodDescriptor.Parameters, payloadProvider)))
                                .Where(methodList => methodList.Count() > 0)
                                .ToDictionary(methodList => methodList.First().Hub.Name,
                                                methodList => methodList
                                                .Select(methodDescriptor =>
                                                    new object[]{
                                                                    methodDescriptor.Name,
                                                                    methodDescriptor.Parameters
                                                                                    .Select(parameterDescriptor => 
                                                                                        {
                                                                                            PayloadDescriptor payloadDescriptor = payloadProvider.GetPayload(parameterDescriptor.ParameterType);
                                                                                            bool enumerable = false;
                                                                                            long payloadId = -1;

                                                                                            // If payloadDescriptor is null then the parameter type may have a payload within it
                                                                                            if (payloadDescriptor == null)
                                                                                            {
                                                                                                // See if parameter is enumerable
                                                                                                if(parameterDescriptor.ParameterType.IsEnumerable())
                                                                                                {
                                                                                                    enumerable = true;
                                                                                                    payloadDescriptor = payloadProvider.GetPayload(parameterDescriptor.ParameterType.GetEnumerableType());

                                                                                                    if(payloadDescriptor != null)
                                                                                                    {
                                                                                                        payloadId = payloadDescriptor.ID;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                payloadId = payloadDescriptor.ID;
                                                                                            }

                                                                                            return new object[]{
                                                                                                payloadId,
                                                                                                enumerable
                                                                                            };
                                                                                        })
                                                                }).ToDictionary(methodNameToID => methodNameToID[0],
                                                                                methodNameToID => methodNameToID[1]));
        }

        private static object CreatePayloadContracts(IPayloadDescriptorProvider payloadProvider)
        {
            return payloadProvider.GetPayloads()
                                    .Select(payloadDescriptor => payloadDescriptor)
                                    .ToDictionary(payloadDescriptor => payloadDescriptor.ID,
                                                    payloadDescriptor => payloadDescriptor.Data
                                                                        .Select(dataDescriptor =>
                                                                        {
                                                                            PayloadDescriptor datasPayloadDescriptor = payloadProvider.GetPayload(dataDescriptor.Type);
                                                                            bool enumerable = false;
                                                                            long payloadId = -1;

                                                                            // If payloadDescriptor is null then the parameter type may have a payload within it
                                                                            if (datasPayloadDescriptor == null)
                                                                            {
                                                                                // See if parameter is enumerable
                                                                                if (dataDescriptor.Type.IsEnumerable())
                                                                                {
                                                                                    enumerable = true;
                                                                                    datasPayloadDescriptor = payloadProvider.GetPayload(dataDescriptor.Type.GetEnumerableType());

                                                                                    if (datasPayloadDescriptor != null)
                                                                                    {
                                                                                        payloadId = datasPayloadDescriptor.ID;
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                payloadId = datasPayloadDescriptor.ID;
                                                                            }

                                                                            return new object[] {
                                                                                dataDescriptor.Name,
                                                                                new object[]{
                                                                                    payloadId,
                                                                                    enumerable
                                                                                }
                                                                            };
                                                                        }));
        }

        private static object CreateContractsCache(IJsonSerializer serializer, IPayloadDescriptorProvider payloadProvider, IMethodDescriptorProvider methodProvider, IHubDescriptorProvider hubProvider)
        {
            var methodReturnContracts = CreateMethodReturnContracts(payloadProvider, methodProvider, hubProvider);

            var methodInvokerContracts = CreateMethodInvokerContracts(payloadProvider, methodProvider, hubProvider);

            var payloadContracts = CreatePayloadContracts(payloadProvider);
            // Format is:
            // [0] = Methods that return a contractable type (Used for Hub Responses)
            // [1] = Payload contracts
            return new object[] { methodReturnContracts, methodInvokerContracts, payloadContracts };
        }

        public object GenerateContracts()
        {
            return _generatedContracts.Value;
        }

        private static bool HasPayloadArgument(IEnumerable<ParameterDescriptor> arguments, IPayloadDescriptorProvider payloadProvider)
        {
            try
            {
                return arguments.Where(parameterDescriptor => payloadProvider.HasPayload(parameterDescriptor.ParameterType))
                                .Count() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
