using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    public class PayloadCompressionModule : HubPipelineModule
    {
        private IPayloadCompressor _compressor;
        private IPayloadDescriptorProvider _provider;
        private IContractsGenerator _generator;

        public PayloadCompressionModule()
            : this(compressor: null, provider: null, generator: null)
        {
        }

        public PayloadCompressionModule(IPayloadCompressor compressor, IPayloadDescriptorProvider provider, IContractsGenerator generator)
        {
            _compressor = compressor;
            _provider = provider;
            _generator = generator;
        }

        public override Func<IHubIncomingInvokerContext, Task<object>> BuildIncoming(Func<IHubIncomingInvokerContext, Task<object>> invoke)
        {
            return base.BuildIncoming((context) =>
            {
                var taskResult = invoke(context);
                var result = ((Task<object>)taskResult).Result;
                PayloadDescriptor descriptor = null;

                if (result != null)
                {
                    var type = result.GetType();
                    descriptor = _provider.GetPayload(type) ?? ((type.IsEnumerable()) ? _provider.GetPayload(type.GetEnumerableType()) : null);
                }

                if (descriptor == null)
                {
                    return taskResult.Then(r => _compressor.Compress(r));
                }
                else
                {
                    return taskResult.Then(r => _compressor.Compress(r, descriptor.Settings));
                }
            });
        }

        public override Func<IHubOutgoingInvokerContext, Task> BuildOutgoing(Func<IHubOutgoingInvokerContext, Task> send)
        {
            return base.BuildOutgoing((context) =>
            {
                var args = context.Invocation.Args;
                string[] contracts = new string[args.Length];

                for (var i = 0; i < args.Length; i++)
                {
                    long contractId = -1;
                    bool enumerable = false;
                    Type argType = args[i].GetType();
                    PayloadDescriptor descriptor;

                    if (argType.IsEnumerable())
                    {
                        enumerable = true;
                        descriptor = _provider.GetPayload(argType.GetEnumerableType());
                    }
                    else
                    {
                        descriptor = _provider.GetPayload(args[i].GetType());
                    }

                    // If there's a descriptor for the given arg we can compress it
                    if (descriptor != null)
                    {
                        args[i] = _compressor.Compress(args[i], descriptor.Settings);
                        contractId = descriptor.ID;
                    }
                    else
                    {
                        // Don't want to send down any unnecessary data even if the current object is enumerable
                        enumerable = false;
                    }

                    contracts[i] = (enumerable) ? contractId.ToString() + "[]" : contractId.ToString();
                }

                context.Invocation.Args = args;

                var invocation = new ContractedClientHubInvocation(context.Invocation)
                {
                    ContractIds = contracts
                };

                return send(new ExternalHubOutgoingInvokerContext(context.Connection, context.Signal, invocation, context.ExcludedSignals));
            });
        }
    }
}
