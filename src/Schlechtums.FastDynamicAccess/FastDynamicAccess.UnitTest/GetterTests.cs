using FastDynamicAccess.UnitTest.Types;
using System;
using System.Runtime.CompilerServices;
using Xunit;

[assembly: InternalsVisibleTo("FastDynamicAccessAccessors")]
namespace FastDynamicAccess.UnitTest
{
    public class GetterTests
    {

        #region <<< Object >>>
        [Fact]
        public void GetString_Test()
        {
            Type type = typeof(MyClass);
            var expected = "Hello World";
            var m = new MyClass { TestString = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestString");

            Assert.Equal(expected, (String)retValue);
        }

        [Fact]
        public void GetBoolean_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestBoolean = true };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestBoolean");

            Assert.True((Boolean)retValue);
        }

        [Fact]
        public void GetByte_Test()
        {
            Type type = typeof(MyClass);
            var expected = (Byte)5;
            var m = new MyClass { TestByte = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestByte");

            Assert.Equal<Byte>(expected, (Byte)retValue);
        }

        [Fact]
        public void GetShort_Test()
        {
            Type type = typeof(MyClass);
            var expected = (short)5;
            var m = new MyClass { TestShort = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestShort");

            Assert.Equal<short>(expected, (short)retValue);
        }

        [Fact]
        public void GetInt_Test()
        {
            Type type = typeof(MyClass);
            var expected = 5;
            var m = new MyClass { TestInt = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestInt");

            Assert.Equal<int>(expected, (int)retValue);
        }

        [Fact]
        public void GetFloat_Test()
        {
            Type type = typeof(MyClass);
            var expected = 5f;
            var m = new MyClass { TestFloat = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestFloat");

            Assert.Equal<float>(expected, (float)retValue);
        }

        [Fact]
        public void GetDouble_Test()
        {
            Type type = typeof(MyClass);
            var expected = 5d;
            var m = new MyClass { TestDouble = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestDouble");

            Assert.Equal<Double>(expected, (Double)retValue);
        }

        [Fact]
        public void GetDecimal_Test()
        {
            Type type = typeof(MyClass);
            var expected = 5m;
            var m = new MyClass { TestDecimal = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestDecimal");

            Assert.Equal<Decimal>(expected, (Decimal)retValue);
        }

        [Fact]
        public void GetDateTime_Test()
        {
            Type type = typeof(MyClass);
            var expected = new DateTime(1900, 3, 11);
            var m = new MyClass { TestDateTime = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestDateTime");

            Assert.Equal<DateTime>(expected, (DateTime)retValue);

            var actualYear = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(retValue).GetValue<int>(retValue, "Year");
            Assert.Equal<int>(expected.Year, actualYear);
        }

        [Fact]
        public void GetTimeSpan_Test()
        {
            Type type = typeof(MyClass);
            var expected = new TimeSpan(18, 21, 31);
            var m = new MyClass { TestTimeSpan = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestTimeSpan");

            Assert.Equal<TimeSpan>(expected, (TimeSpan)retValue);
        }

        [Fact]
        public void GetByteArray_Test()
        {
            Type type = typeof(MyClass);
            var expected = new Byte[] { 7, 8, 9 };
            var m = new MyClass { TestByteArray = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestByteArray");

            Assert.Equal<int>(expected.Length, ((Byte[])retValue).Length);
            Assert.Equal<Byte[]>(expected, (Byte[])retValue);
            Assert.Equal<Byte>(expected[0], ((Byte[])retValue)[0]);
            Assert.Equal<Byte>(expected[1], ((Byte[])retValue)[1]);
            Assert.Equal<Byte>(expected[2], ((Byte[])retValue)[2]);
        }

        [Fact]
        public void GetByteArray_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestByteArray = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestByteArray");

            Assert.Null((Byte[])retValue);
        }

        [Fact]
        public void GetBooleanNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableBoolean = (Boolean?)true };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableBoolean");

            Assert.True((Boolean?)retValue);
        }

        [Fact]
        public void GetByteNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (Byte?)5;
            var m = new MyClass { TestNullableByte = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableByte");

            Assert.Equal<Byte?>(expected, (Byte?)retValue);
        }

        [Fact]
        public void GetShortNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (short?)5;
            var m = new MyClass { TestNullableShort = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableShort");

            Assert.Equal<short?>(expected, (short?)retValue);
        }

        [Fact]
        public void GetIntNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (int?)5;
            var m = new MyClass { TestNullableInt = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableInt");

            Assert.Equal<int?>(expected, (int?)retValue);
        }

        [Fact]
        public void GetFloatNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (float?)5f;
            var m = new MyClass { TestNullableFloat = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableFloat");

            Assert.Equal<float?>(expected, (float?)retValue);
        }

        [Fact]
        public void GetDoubleNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (Double?)5d;
            var m = new MyClass { TestNullableDouble = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableDouble");

            Assert.Equal<Double?>(expected, (Double?)retValue);
        }

        [Fact]
        public void GetDecimalNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (Decimal?)5m;
            var m = new MyClass { TestNullableDecimal = 5m };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableDecimal");

            Assert.Equal<Decimal?>(expected, (Decimal?)retValue);
        }

        [Fact]
        public void GetDateTimeNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (DateTime?)new DateTime(1900, 3, 11);
            var m = new MyClass { TestNullableDateTime = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableDateTime");

            Assert.Equal<DateTime?>(expected, (DateTime?)retValue);
        }

        [Fact]
        public void GetTimeSpanNullableTest()
        {
            Type type = typeof(MyClass);
            var expected = (TimeSpan?)new TimeSpan(18, 21, 31);
            var m = new MyClass { TestNullableTimeSpan = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableTimeSpan");

            Assert.Equal<TimeSpan?>(expected, (TimeSpan?)retValue);
        }

        [Fact]
        public void GetBooleanNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableBoolean = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableBoolean");

            Assert.Null((Boolean?)retValue);
        }

        [Fact]
        public void GetByteNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableByte = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableByte");

            Assert.Null((Byte?)retValue);
        }

        [Fact]
        public void GetShortNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableShort = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableShort");

            Assert.Null((short?)retValue);
        }

        [Fact]
        public void GetIntNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableInt = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableInt");

            Assert.Null((int?)retValue);
        }

        [Fact]
        public void GetFloatNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableFloat = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableFloat");

            Assert.Null((float?)retValue);
        }

        [Fact]
        public void GetDoubleNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDouble = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableDouble");

            Assert.Null((Double?)retValue);
        }

        [Fact]
        public void GetDecimalNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDecimal = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableDecimal");

            Assert.Null((Decimal?)retValue);
        }

        [Fact]
        public void GetDateTimeNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDateTime = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableDateTime");

            Assert.Null((DateTime?)retValue);
        }

        [Fact]
        public void GetTimeSpanNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableTimeSpan = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue(m, "TestNullableTimeSpan");

            Assert.Null((TimeSpan?)retValue);
        }
        #endregion

        #region <<< Generic >>>
        [Fact]
        public void GetStringGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestString = "Hello World" };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<String>(m, "TestString");

            Assert.Equal("Hello World", retValue);
        }

        [Fact]
        public void GetBooleanGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestBoolean = true };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Boolean>(m, "TestBoolean");

            Assert.True(retValue);
        }

        [Fact]
        public void GetByteGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestByte = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Byte>(m, "TestByte");

            Assert.Equal<Byte>(5, retValue);
        }

        [Fact]
        public void GetShortGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestShort = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<short>(m, "TestShort");

            Assert.Equal<short>(5, retValue);
        }

        [Fact]
        public void GetIntGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestInt = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<int>(m, "TestInt");

            Assert.Equal<int>(5, retValue);
        }

        [Fact]
        public void GetFloatGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestFloat = 5f };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<float>(m, "TestFloat");

            Assert.Equal<float>(5f, retValue);
        }

        [Fact]
        public void GetDoubleGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestDouble = 5d };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<double>(m, "TestDouble");

            Assert.Equal<Double>(5d, retValue);
        }

        [Fact]
        public void GetDecimalGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestDecimal = 5m };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<decimal>(m, "TestDecimal");

            Assert.Equal<Decimal>(5m, retValue);
        }

        [Fact]
        public void GetDateTimeGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = new DateTime(1900, 3, 11);
            var m = new MyClass { TestDateTime = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<DateTime>(m, "TestDateTime");

            Assert.Equal<DateTime>(expected, retValue);

            var actualYear = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(retValue).GetValue<int>(retValue, "Year");
            Assert.Equal<int>(expected.Year, actualYear);
        }

        [Fact]
        public void GetTimeSpanGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = new TimeSpan(18, 21, 31);
            var m = new MyClass { TestTimeSpan = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<TimeSpan>(m, "TestTimeSpan");

            Assert.Equal<TimeSpan>(expected, retValue);
        }

        [Fact]
        public void GetByteArrayGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = new Byte[] { 7, 8, 9 };
            var m = new MyClass { TestByteArray = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Byte[]>(m, "TestByteArray");

            Assert.Equal<int>(3, retValue.Length);
            Assert.Equal<Byte[]>(expected, retValue);
            Assert.Equal<Byte>(7, retValue[0]);
            Assert.Equal<Byte>(8, retValue[1]);
            Assert.Equal<Byte>(9, retValue[2]);
        }

        [Fact]
        public void GetByteArray_Null_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestByteArray = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Byte[]>(m, "TestByteArray");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetBooleanNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableBoolean = true };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Boolean?>(m, "TestNullableBoolean");

            Assert.True(retValue);
        }

        [Fact]
        public void GetByteNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (Byte?)5;
            var m = new MyClass { TestNullableByte = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Byte?>(m, "TestNullableByte");

            Assert.Equal<Byte?>(expected, (Byte)retValue);
        }

        [Fact]
        public void GetShortNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (short?)5;
            var m = new MyClass { TestNullableShort = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<short?>(m, "TestNullableShort");

            Assert.Equal<short?>(expected, retValue);
        }

        [Fact]
        public void GetIntNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (int?)5;
            var m = new MyClass { TestNullableInt = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<int?>(m, "TestNullableInt");

            Assert.Equal<int?>(expected, retValue);
        }

        [Fact]
        public void GetFloatNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (float?)5f;
            var m = new MyClass { TestNullableFloat = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<float?>(m, "TestNullableFloat");

            Assert.Equal<float?>(expected, retValue);
        }

        [Fact]
        public void GetDoubleNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (Double?)5d;
            var m = new MyClass { TestNullableDouble = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Double?>(m, "TestNullableDouble");

            Assert.Equal<Double?>(expected, retValue);
        }

        [Fact]
        public void GetDecimalNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (Decimal?)5m;
            var m = new MyClass { TestNullableDecimal = expected};
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Decimal?>(m, "TestNullableDecimal");

            Assert.Equal<Decimal?>(expected, retValue);
        }

        [Fact]
        public void GetDateTimeNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (DateTime?)new DateTime(1900, 3, 11);
            var m = new MyClass { TestNullableDateTime = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<DateTime?>(m, "TestNullableDateTime");

            Assert.Equal<DateTime?>(expected, retValue);
        }

        [Fact]
        public void GetTimeSpanNullableGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = (TimeSpan?)new TimeSpan(18, 21, 31);
            var m = new MyClass { TestNullableTimeSpan = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<TimeSpan?>(m, "TestNullableTimeSpan");

            Assert.Equal<TimeSpan?>(expected, retValue);
        }

        [Fact]
        public void GetBooleanNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableBoolean = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Boolean?>(m, "TestNullableBoolean");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetByteNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableByte = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Byte?>(m, "TestNullableByte");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetShortNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableShort = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<short?>(m, "TestNullableShort");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetIntNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableInt = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<int?>(m, "TestNullableInt");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetFloatNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableFloat = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<float?>(m, "TestNullableFloat");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetDoubleNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDouble = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Double?>(m, "TestNullableDouble");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetDecimalNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDecimal = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<Decimal?>(m, "TestNullableDecimal");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetDateTimeNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDateTime = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<DateTime?>(m, "TestNullableDateTime");

            Assert.Null(retValue);
        }

        [Fact]
        public void GetTimeSpanNullableNull_Generic_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableTimeSpan = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            var retValue = fda.GetValue<TimeSpan?>(m, "TestNullableTimeSpan");

            Assert.Null(retValue);
        }
        #endregion
    }
}