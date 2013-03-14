using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace SignalR.Compression.Server
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

        private object ApplyCompressionSettings(object payload, CompressionSettings settings)
        {
            var payloadType = payload.GetType();

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

            return payload;
        }

        private object CheckNull(object payload, DataDescriptor descriptor)
        {
            if (payload == null)
            {
                if (descriptor.CompressionTypeId != CompressionTypeHelper.NumericTypeId)
                {
                    return 0;
                }
            }

            return payload;
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
                                return Compress(CheckNull(dataDescriptor.GetValue(payload), dataDescriptor), settings);
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

                payload = ApplyCompressionSettings(payload, settings);
            }

            return payload;
        }

        public object Compress(object payload)
        {
            return Compress(payload, CompressionSettings.Default);
        }
    }
}
