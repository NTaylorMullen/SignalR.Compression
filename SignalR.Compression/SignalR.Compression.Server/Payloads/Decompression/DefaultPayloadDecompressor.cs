using System;
using System.Collections;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;

namespace SignalR.Compression.Server
{
    public class DefaultPayloadDecompressor : IPayloadDecompressor
    {
        private IPayloadDescriptorProvider _provider;

        public DefaultPayloadDecompressor(IDependencyResolver resolver)
            : this(resolver.Resolve<IPayloadDescriptorProvider>())
        {
        }

        public DefaultPayloadDecompressor(IPayloadDescriptorProvider provider)
        {
            _provider = provider;
        }

        private object CheckNull(object payload, DataDescriptor descriptor)
        {
            if (descriptor.CompressionTypeId != CompressionTypeHelper.NumericTypeId && payload != null)
            {
                long value;
                if (long.TryParse(payload.ToString(), out value) && value == 0)
                {
                    return null;
                }
            }
            return payload;
        }

        public object Decompress(object payload, PayloadDescriptor payloadDescriptor)
        {
            var compressedPayload = payload as object[];
            var result = Activator.CreateInstance(payloadDescriptor.Type);
            var i = 0;

            // TODO: Throw error if the compressedPayload length != payloadDescriptor.Data length
            foreach (var data in payloadDescriptor.Data)
            {
                var value = compressedPayload[i++];

                // Convert value to null if it was minified as a 0
                value = CheckNull(value, data);

                if (value != null)
                {
                    if (_provider.IsPayload(data.Type))
                    {
                        data.SetValue(result, Decompress((value as JArray).ToObject<object[]>(), data.Type));
                    }
                    else
                    {
                        if (value.GetType() != data.Type)
                        {
                            value = Convert.ChangeType(value, data.Type);
                        }

                        data.SetValue(result, value);
                    }
                }
            }

            return result;
        }

        public object Decompress(object payload, Type expected)
        {
            if (payload != null)
            {
                PayloadDescriptor payloadDescriptor;
                var enumerable = false;

                if (expected.IsEnumerable())
                {
                    enumerable = true;
                    payloadDescriptor = _provider.GetPayload(expected.GetEnumerableType());
                }
                else
                {
                    payloadDescriptor = _provider.GetPayload(expected);
                }

                // Check if our payload object is actually a payload
                if (payloadDescriptor != null)
                {
                    if (enumerable)
                    {
                        var list = Activator.CreateInstance(expected) as IList;
                        var compressedPayload = payload as object[];

                        foreach (object data in compressedPayload)
                        {
                            list.Add(Decompress((data as JArray).ToObject<object[]>(), payloadDescriptor));
                        }

                        return list;
                    }
                    else
                    {
                        return Decompress(payload, payloadDescriptor);
                    }
                }
            }

            return payload;
        }
    }
}
