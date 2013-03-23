
namespace Microsoft.AspNet.SignalR.Compression.Server
{
    internal static class CompressionTypeHelper
    {
        public const int DefaultTypeId = 0;
        public const int EnumerableTypeId = 1;
        public const int NumericTypeId = 2;

        public static int GetCompressionType(DataDescriptor dataDescriptor)
        {
            if (dataDescriptor.Enumerable)
            {
                return EnumerableTypeId;
            }
            else if (dataDescriptor.Type.IsNumeric())
            {
                return NumericTypeId;
            }

            return DefaultTypeId;
        }
    }
}
