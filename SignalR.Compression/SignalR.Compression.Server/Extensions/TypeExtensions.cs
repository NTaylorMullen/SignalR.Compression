using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalR.Compression
{
    public static class TypeExtensions
    {
        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces()
                        .Where(t => t.IsGenericType)
                        .Where(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .Count() > 0;
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
    }
}
