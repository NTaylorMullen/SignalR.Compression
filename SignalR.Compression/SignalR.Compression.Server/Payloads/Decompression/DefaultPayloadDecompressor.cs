using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;

namespace SignalR.Compression.Server
{
    internal class DefaultPayloadDecompressor : IPayloadDecompressor
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
                    if (_provider.HasPayload(data.Type))
                    {
                        data.SetValue(result, Decompress(ConvertToObjectArray(value), data.Type));
                    }
                    else // We're not directly a payload at this points
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
                        IList list = null;

                        // Arrays have no default constructor so cannot be created by the Activator
                        if (expected.IsArray)
                        {
                            list = new List<object>();
                        }
                        else
                        {
                            list = Activator.CreateInstance(expected) as IList;
                        }

                        var compressedPayload = payload as object[];

                        foreach (object data in compressedPayload)
                        {
                            list.Add(Decompress(ConvertToObjectArray(data), payloadDescriptor));
                        }

                        if (expected.IsArray)
                        {
                            var arr = Array.CreateInstance(expected.GetEnumerableType(), list.Count);

                            list.CopyTo(arr, 0);

                            return arr;
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

        private object[] ConvertToObjectArray(object data)
        {
            if (data.GetType() != typeof(JArray))
            {
                return data as object[];
            }

            return (data as JArray).ToObject<object[]>();
        }
    }
}
