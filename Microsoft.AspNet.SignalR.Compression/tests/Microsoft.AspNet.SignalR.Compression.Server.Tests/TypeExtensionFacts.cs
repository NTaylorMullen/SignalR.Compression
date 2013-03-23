using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Microsoft.AspNet.SignalR.Compression.Server.Tests
{
    public class TypeExtensionFacts
    {
        [Theory]
        [InlineData(typeof(IList<int>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(List<int>[]))]
        [InlineData(typeof(ICollection<int>[]))]
        [InlineData(typeof(Queue<int>))]
        [InlineData(typeof(Stack<int>))]
        public void IsEnumerableDetectsMultipleTypesOfCollections(Type type)
        {
            Assert.True(type.IsEnumerable());
        }

        // We don't want isEnumerable to pick up generic collections
        [Theory]
        [InlineData(typeof(ArrayList))]
        [InlineData(typeof(Array))]
        [InlineData(typeof(String))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Queue))]
        [InlineData(typeof(Stack))]
        [InlineData(typeof(ICollection))]
        [InlineData(typeof(IList))]
        public void IsEnumerableDoesNotDetectCertainTypesOfCollections(Type type)
        {
            Assert.False(type.IsEnumerable());
        }

        [Theory]
        [InlineData(typeof(IList<int>), typeof(int))]
        [InlineData(typeof(List<string>), typeof(string))]
        [InlineData(typeof(object[]), typeof(object))]
        [InlineData(typeof(short[]), typeof(short))]
        [InlineData(typeof(List<bool>[]), typeof(bool))]
        [InlineData(typeof(ICollection<IList<int>>[]), typeof(IList<int>))]
        [InlineData(typeof(Queue<long>), typeof(long))]
        [InlineData(typeof(Stack<double>), typeof(double))]
        public void GetEnumerableTypePullsTypeCorrectly(Type enumerable, Type genericType)
        {
            Assert.Equal(enumerable.GetEnumerableType(), genericType);
        }

        [Theory]
        [InlineData(typeof(double))]
        [InlineData(typeof(Double))]
        [InlineData(typeof(Decimal))]
        [InlineData(typeof(decimal))]
        public void CanBeRound(Type type)
        {
            Assert.True(type.CanBeRounded());
        }

        [Theory]
        [InlineData(typeof(short))]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(string))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(object))]
        public void CantBeRound(Type type)
        {
            Assert.False(type.CanBeRounded());
        }

        [Theory]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(short))]
        [InlineData(typeof(int))]
        [InlineData(typeof(long))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(UInt16))]
        [InlineData(typeof(UInt32))]
        [InlineData(typeof(UInt64))]
        [InlineData(typeof(Single))]
        [InlineData(typeof(Byte))]
        [InlineData(typeof(SByte))]
        [InlineData(typeof(Double))]
        [InlineData(typeof(Decimal))]
        public void IsNumeric(Type type)
        {
            Assert.True(type.IsNumeric());
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(char))]
        [InlineData(typeof(String))]
        [InlineData(typeof(Char))]
        [InlineData(typeof(object))]
        [InlineData(typeof(Object))]
        [InlineData(typeof(Boolean))]
        [InlineData(typeof(bool))]
        public void IsNotNumeric(Type type)
        {
            Assert.False(type.IsNumeric());
        }
    }
}

