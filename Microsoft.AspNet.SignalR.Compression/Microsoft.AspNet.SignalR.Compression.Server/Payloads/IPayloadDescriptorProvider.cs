using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    public interface IPayloadDescriptorProvider
    {
        /// <summary>
        /// Retrieve all available payload types.
        /// </summary>
        /// <returns>Collection of payload descriptors.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This call might be expensive")]
        IEnumerable<PayloadDescriptor> GetPayloads();

        /// <summary>
        /// Retrieve individual payload
        /// </summary>
        /// <param name="type">The type of payload descriptor to resolve</param>
        /// <returns>PayloadDescriptor of type T</returns>
        PayloadDescriptor GetPayload(Type type);

        /// <summary>
        /// Determines if a type is a payload type
        /// </summary>
        /// <param name="type">The type to investigate</param>
        /// <returns>Whether the type is a payload type or not</returns>
        bool IsPayload(Type type);

        /// <summary>
        /// Determines if a type has a payload within it
        /// </summary>
        /// <param name="type">The type to investigate</param>
        /// <returns>Whether the type has a payload type or not</returns>
        bool HasPayload(Type type);
    }
}
