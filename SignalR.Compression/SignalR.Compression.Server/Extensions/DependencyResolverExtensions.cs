using Microsoft.AspNet.SignalR;

namespace SignalR.Compression.Server
{
    public static class DependencyResolverExtensions
    {
        private static CompressionBase _compressionBase = new CompressionBase();

        public static CompressionBase Compression(this IDependencyResolver resolver)
        {
            return _compressionBase;
        }
    }
}
