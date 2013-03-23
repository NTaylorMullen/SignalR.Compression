using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Json;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    internal class CompressableParameterResolver : IParameterResolver
    {
        private IPayloadDescriptorProvider _provider;
        private IPayloadDecompressor _decompressor;

        public CompressableParameterResolver(IPayloadDescriptorProvider provider, IPayloadDecompressor decompressor)
        {
            _provider = provider;
            _decompressor = decompressor;
        }

        /// <summary>
        /// Resolves a parameter value based on the provided object.
        /// </summary>
        /// <param name="descriptor">Parameter descriptor.</param>
        /// <param name="value">Value to resolve the parameter value from.</param>
        /// <returns>The parameter value.</returns>
        public virtual object ResolveParameter(ParameterDescriptor descriptor, IJsonValue value)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (_provider.HasPayload(descriptor.ParameterType))
            {
                return _decompressor.Decompress(value.ConvertTo(typeof(object[])), descriptor.ParameterType);
            }
            else
            {
                return value.ConvertTo(descriptor.ParameterType);
            }
        }

        public IList<object> ResolveMethodParameters(MethodDescriptor method, IList<IJsonValue> values)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            return method.Parameters.Zip(values, ResolveParameter).ToArray();
        }
    }
}
