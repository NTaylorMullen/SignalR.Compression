using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    internal class ReflectedPayloadDescriptorProvider : IPayloadDescriptorProvider
    {
        private readonly Lazy<IDictionary<Type, PayloadDescriptor>> _payloads;
        private readonly Lazy<IAssemblyLocator> _locator;

        private static long _payloadDescriptorID = 0;

        public ReflectedPayloadDescriptorProvider(IDependencyResolver resolver)
        {
            _locator = new Lazy<IAssemblyLocator>(resolver.Resolve<IAssemblyLocator>);
            _payloads = new Lazy<IDictionary<Type, PayloadDescriptor>>(BuildPayloadsCache);
        }

        protected IDictionary<Type, PayloadDescriptor> BuildPayloadsCache()
        {
            // Getting all payloads that have a payload attribute
            var types = _locator.Value.GetAssemblies()
                        .SelectMany(GetTypesSafe)
                        .Where(HasPayloadAttribute);

            // Building cache entries for each descriptor
            // Each descriptor is stored in dictionary under a key
            // that is it's name
            var cacheEntries = types
                .Select(type => new PayloadDescriptor
                {
                    Type = type,
                    ID = Interlocked.Increment(ref _payloadDescriptorID),
                    Settings = new CompressionSettings
                    {
                        RoundNumbersTo = ((PayloadAttribute)Attribute.GetCustomAttribute(type, typeof(PayloadAttribute))).RoundNumbersTo
                    },
                    Data = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Select(propertyInfo =>
                                {
                                    var descriptor = new DataDescriptor
                                        {
                                            Name = propertyInfo.Name,
                                            Type = propertyInfo.PropertyType,
                                            Enumerable = propertyInfo.PropertyType.IsEnumerable(),
                                            SetValue = (baseObject, newValue) =>
                                            {
                                                propertyInfo.SetValue(baseObject, newValue, null);
                                            },
                                            GetValue = (baseObject) =>
                                            {
                                                return propertyInfo.GetValue(baseObject, null);
                                            }
                                        };

                                    descriptor.CompressionTypeId = CompressionTypeHelper.GetCompressionType(descriptor);

                                    return descriptor;
                                })
                               .Union(type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                               .Select(fieldInfo =>
                                {
                                    var descriptor = new DataDescriptor
                                        {
                                            Name = fieldInfo.Name,
                                            Type = fieldInfo.FieldType,
                                            Enumerable = fieldInfo.FieldType.IsEnumerable(),
                                            SetValue = (baseObject, newValue) =>
                                            {
                                                fieldInfo.SetValue(baseObject, newValue);
                                            },
                                            GetValue = (baseObject) =>
                                            {
                                                return fieldInfo.GetValue(baseObject);
                                            }
                                        };

                                    descriptor.CompressionTypeId = CompressionTypeHelper.GetCompressionType(descriptor);

                                    return descriptor;
                                }))
                               .OrderBy(dataDescriptor => dataDescriptor.Name)
                })
                .ToDictionary(payload => payload.Type,
                              payload => payload);

            return cacheEntries;
        }

        public IEnumerable<PayloadDescriptor> GetPayloads()
        {
            return _payloads.Value
                .Select(a => a.Value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "If we throw then we have an empty type")]
        private static IEnumerable<Type> GetTypesSafe(Assembly a)
        {
            try
            {
                return a.GetTypes();
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }

        private static bool HasPayloadAttribute(Type type)
        {
            try
            {
                return Attribute.IsDefined(type, typeof(PayloadAttribute));
            }
            catch
            {
                return false;
            }
        }

        public PayloadDescriptor GetPayload(Type type)
        {
            if (IsPayload(type))
            {
                return _payloads.Value[type];
            }

            return null;
        }

        public bool IsPayload(Type type)
        {
            return _payloads.Value.Keys.Contains(type);
        }

        public bool HasPayload(Type type)
        {
            if (type.IsEnumerable())
            {
                type = type.GetEnumerableType();
            }

            return IsPayload(type);
        }
    }
}
