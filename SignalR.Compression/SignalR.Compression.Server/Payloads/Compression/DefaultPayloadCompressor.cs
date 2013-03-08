using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace SignalR.Compression
{
    public class DefaultPayloadCompressor : IPayloadCompressor
    {
        private IPayloadDescriptorProvider _provider;

        public DefaultPayloadCompressor(IDependencyResolver resolver)
            : this(resolver.Resolve<IPayloadDescriptorProvider>())
        {
        }

        public DefaultPayloadCompressor(IPayloadDescriptorProvider provider)
        {
            _provider = provider;
        }

        public object Compress(object payload, CompressionSettings settings)
        {
            if (payload != null)
            {
                var payloadType = payload.GetType();
                var payloadDescriptor = _provider.GetPayload(payloadType);

                // Only compress the payload if we have a payload descriptor for it
                if (payloadDescriptor != null)
                {
                    return payloadDescriptor.Data.Select(dataDescriptor =>
                            {
                                // Recursively compress the object value until it's at a base type
                                return Compress(dataDescriptor.GetValue(payload), settings);
                            });
                }
                else
                {
                    // At this point the payload object isn't directly a payload but may contain a payload
                    if (_provider.HasPayload(payloadType))
                    {
                        payloadType = payloadType.GetEnumerableType();
                        var itemType = payload.GetType();
                        payloadDescriptor = _provider.GetPayload(payloadType);
                        var payloadList = payload as ICollection;
                        var compressedList = new List<object>();

                        if (payloadDescriptor != null)
                        {
                            foreach (var item in payloadList)
                            {
                                compressedList.Add(Compress(item, payloadDescriptor.Settings));
                            }

                            return compressedList;
                        }
                    }
                }

                payloadType = payload.GetType();

                if (settings.DigitsToMaintain >= 0 && payloadType.CanBeRounded())
                {
                    if (payloadType != typeof(double))
                    {
                        payload = Math.Round((decimal)payload, settings.DigitsToMaintain);
                    }
                    else
                    {
                        payload = Math.Round((double)payload, settings.DigitsToMaintain);
                    }
                }
            }

            return payload;
        }

        public object Compress(object payload)
        {
            return Compress(payload, CompressionSettings.Default);
        }
    }
}
