using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNet.SignalR.Compression.Server
{
    internal static class TypeExtensions
    {
        public static bool IsEnumerable(this Type type)
        {
            return type != typeof(String) &&
                        (type.GetInterfaces()
                        .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .Count() > 0);
        }

        public static Type GetEnumerableType(this Type type)
        {
            Type[] typeList = type.GetGenericArguments();

            if (typeList.Length > 0)
            {
                return type.GetGenericArguments()[0];
            }
            else if (type.IsArray)
            {
                return type.GetElementType();
            }

            return typeof(object);
        }

        public static bool CanBeRounded(this Type type)
        {
            if (type == typeof(double) || type == typeof(decimal))
            {
                return true;
            }

            return false;
        }

        public static bool IsNumeric(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
