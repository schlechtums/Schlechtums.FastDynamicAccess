using System;
using System.Runtime.CompilerServices;
using Xunit;

[assembly: InternalsVisibleTo("FastDynamicAccessAccessors")]
namespace FastDynamicAccess.UnitTest
{
    public class FastDynamicAccessTest
    {
        internal class MyClass
        {
            internal String TestString { get; set; }
            internal Boolean TestBoolean { get; set; }
            internal Byte TestByte { get; set; }
            internal short TestShort { get; set; }
            internal int TestInt { get; set; }
            internal float TestFloat { get; set; }
            internal Double TestDouble { get; set; }
            internal Decimal TestDecimal { get; set; }
            internal DateTime TestDateTime { get; set; }
            internal TimeSpan TestTimeSpan { get; set; }

            internal Byte[] TestByteArray { get; set; }

            internal Boolean? TestNullableBoolean { get; set; }
            internal Byte? TestNullableByte { get; set; }
            internal short? TestNullableShort { get; set; }
            internal int? TestNullableInt { get; set; }
            internal float? TestNullableFloat { get; set; }
            internal Double? TestNullableDouble { get; set; }
            internal Decimal? TestNullableDecimal { get; set; }
            internal DateTime? TestNullableDateTime { get; set; }
            internal TimeSpan? TestNullableTimeSpan { get; set; }
        }

        #region Getters
        [Fact]
        public void GetStringTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestString = "Hello World" };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestString");

            Assert.Equal("Hello World", (String)retValue);
        }

        [Fact]
        public void GetBooleanTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestBoolean = true };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestBoolean");

            Assert.True((Boolean)retValue);
        }

        [Fact]
        public void GetByteTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestByte = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestByte");

            Assert.Equal<Byte>(5, (Byte)retValue);
        }

        [Fact]
        public void GetShortTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestShort = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestShort");

            Assert.Equal<short>(5, (short)retValue);
        }

        [Fact]
        public void GetIntTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestInt = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestInt");

            Assert.Equal<int>(5, (int)retValue);
        }

        [Fact]
        public void GetFloatTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestFloat = 5f };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestFloat");

            Assert.Equal<float>(5f, (float)retValue);
        }

        [Fact]
        public void GetDoubleTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestDouble = 5d };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestDouble");

            Assert.Equal<Double>(5d, (Double)retValue);
        }

        [Fact]
        public void GetDecimalTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestDecimal = 5m };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestDecimal");

            Assert.Equal<Decimal>(5m, (Decimal)retValue);
        }

        [Fact]
        public void GetDateTimeTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestDateTime = new DateTime(1900, 3, 11) };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestDateTime");

            var expected = new DateTime(1900, 3, 11);
            Assert.Equal<DateTime>(expected, (DateTime)retValue);

            var actualYear = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(retValue).Get<int>(retValue, "Year");
            Assert.Equal<int>(expected.Year, actualYear);
        }

        [Fact]
        public void GetTimeSpanTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestTimeSpan = new TimeSpan(18, 21, 31) };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestTimeSpan");

            Assert.Equal<TimeSpan>(new TimeSpan(18, 21, 31), (TimeSpan)retValue);
        }

        [Fact]
        public void GetByteArrayTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestByteArray = new Byte[] { 7, 8, 9 } };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestByteArray");

            Assert.Equal<int>(3, ((Byte[])retValue).Length);
            Assert.Equal<Byte[]>(m.TestByteArray, (Byte[])retValue);
            Assert.Equal<Byte>(7, ((Byte[])retValue)[0]);
            Assert.Equal<Byte>(8, ((Byte[])retValue)[1]);
            Assert.Equal<Byte>(9, ((Byte[])retValue)[2]);
        }

        [Fact]
        public void GetByteArray_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestByteArray = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestByteArray");

            Assert.Null((Byte[])retValue);
        }

        [Fact]
        public void GetBooleanNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableBoolean = true };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableBoolean");

            Assert.True((Boolean)retValue);
        }

        [Fact]
        public void GetByteNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableByte = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableByte");

            Assert.Equal<Byte>(5, (Byte)retValue);
        }

        [Fact]
        public void GetShortNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableShort = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableShort");

            Assert.Equal<short>(5, (short)retValue);
        }

        [Fact]
        public void GetIntNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableInt = 5 };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableInt");

            Assert.Equal<int>(5, (int)retValue);
        }

        [Fact]
        public void GetFloatNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableFloat = 5f };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableFloat");

            Assert.Equal<float>(5f, (float)retValue);
        }

        [Fact]
        public void GetDoubleNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDouble = 5d };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableDouble");

            Assert.Equal<Double>(5d, (Double)retValue);
        }

        [Fact]
        public void GetDecimalNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDecimal = 5m };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableDecimal");

            Assert.Equal<Decimal>(5m, (Decimal)retValue);
        }

        [Fact]
        public void GetDateTimeNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDateTime = new DateTime(1900, 3, 11) };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableDateTime");

            Assert.Equal<DateTime>(new DateTime(1900, 3, 11), (DateTime)retValue);
        }

        [Fact]
        public void GetTimeSpanNullableTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableTimeSpan = new TimeSpan(18, 21, 31) };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableTimeSpan");

            Assert.Equal<TimeSpan>(new TimeSpan(18, 21, 31), (TimeSpan)retValue);
        }

        [Fact]
        public void GetBooleanNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableBoolean = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableBoolean");

            Assert.Null((Boolean?)retValue);
        }

        [Fact]
        public void GetByteNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableByte = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableByte");

            Assert.Null((Byte?)retValue);
        }

        [Fact]
        public void GetShortNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableShort = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableShort");

            Assert.Null((short?)retValue);
        }

        [Fact]
        public void GetIntNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableInt = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableInt");

            Assert.Null((int?)retValue);
        }

        [Fact]
        public void GetFloatNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableFloat = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableFloat");

            Assert.Null((float?)retValue);
        }

        [Fact]
        public void GetDoubleNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDouble = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableDouble");

            Assert.Null((Double?)retValue);
        }

        [Fact]
        public void GetDecimalNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDecimal = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableDecimal");

            Assert.Null((Decimal?)retValue);
        }

        [Fact]
        public void GetDateTimeNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableDateTime = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableDateTime");

            Assert.Null((DateTime?)retValue);
        }

        [Fact]
        public void GetTimeSpanNullableNull_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass { TestNullableTimeSpan = null };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Object retValue = fda.Get(m, "TestNullableTimeSpan");

            Assert.Null((TimeSpan?)retValue);
        }
        #endregion

        #region Setters
        [Fact]
        public void SetStringTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = "Hello World";

            Assert.Null(m.TestString);

            fda.Set(m, "TestString", testValue);

            Assert.Equal(testValue, m.TestString);
        }

        [Fact]
        public void SetBooleanTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = true;

            Assert.False(m.TestBoolean);

            fda.Set(m, "TestBoolean", testValue);

            Assert.Equal<Boolean>(testValue, m.TestBoolean);
        }

        [Fact]
        public void SetByteTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (Byte)10;

            Assert.Equal<Byte>(0, m.TestByte);

            fda.Set(m, "TestByte", testValue);

            Assert.Equal<int>(testValue, m.TestByte);
        }

        [Fact]
        public void SetIntTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = 10;

            Assert.Equal<int>(0, m.TestInt);

            fda.Set(m, "TestInt", testValue);

            Assert.Equal<int>(testValue, m.TestInt);
        }

        [Fact]
        public void SetFloatTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = 10.5f;

            Assert.Equal<float>(0, m.TestFloat);

            fda.Set(m, "TestFloat", testValue);

            Assert.Equal<float>(testValue, m.TestFloat);
        }

        [Fact]
        public void SetDoubleTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = 10.5d;

            Assert.Equal<Double>(0, m.TestDouble);

            fda.Set(m, "TestDouble", testValue);

            Assert.Equal<Double>(testValue, m.TestDouble);
        }

        [Fact]
        public void SetDecimalTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = 10.5m;

            Assert.Equal<Decimal>(0, m.TestDecimal);

            fda.Set(m, "TestDecimal", testValue);

            Assert.Equal<Decimal>(testValue, m.TestDecimal);
        }

        [Fact]
        public void SetDateTimeTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = DateTime.Now;

            Assert.Equal<DateTime>(new DateTime(), m.TestDateTime);

            fda.Set(m, "TestDateTime", testValue);

            Assert.Equal<DateTime>(testValue, m.TestDateTime);
        }

        [Fact]
        public void SetTimeSpanTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = DateTime.Now.TimeOfDay;

            Assert.Equal<TimeSpan>(new TimeSpan(), m.TestTimeSpan);

            fda.Set(m, "TestTimeSpan", testValue);

            Assert.Equal<TimeSpan>(testValue, m.TestTimeSpan);
        }

        [Fact]
        public void SetByteArrayTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = new Byte[] { 7, 8, 9 };

            Assert.Null(m.TestByteArray);

            fda.Set(m, "TestByteArray", testValue);

            Assert.Equal<int>(3, m.TestByteArray.Length);
            Assert.Equal<Byte[]>(testValue, m.TestByteArray);
            Assert.Equal<Byte>(testValue[0], m.TestByteArray[0]);
            Assert.Equal<Byte>(testValue[1], m.TestByteArray[1]);
            Assert.Equal<Byte>(testValue[2], m.TestByteArray[2]);
        }

        [Fact]
        public void SetByteArray_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            Byte[] testValue = null;

            Assert.Null(m.TestByteArray);

            fda.Set(m, "TestByteArray", testValue);

            Assert.Equal<Byte[]>(testValue, m.TestByteArray);
        }

        [Fact]
        public void SetNullableBooleanTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = true;

            Assert.Null(m.TestNullableBoolean);

            fda.Set(m, "TestNullableBoolean", testValue);

            Assert.Equal<Boolean?>(testValue, m.TestNullableBoolean);
        }

        [Fact]
        public void SetNullableBoolean_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            Boolean? testValue = null;

            Assert.Null(m.TestNullableBoolean);

            fda.Set(m, "TestNullableBoolean", testValue);

            Assert.Equal<Boolean?>(testValue, m.TestNullableBoolean);
        }

        [Fact]
        public void SetNullableByteTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (Byte?)5;

            Assert.Null(m.TestNullableByte);

            fda.Set(m, "TestNullableByte", testValue);

            Assert.Equal<Byte?>(testValue, m.TestNullableByte);
        }

        [Fact]
        public void SetNullableByte_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            Byte? testValue = null;

            Assert.Null(m.TestNullableByte);

            fda.Set(m, "TestNullableByte", testValue);

            Assert.Equal<Byte?>(testValue, m.TestNullableByte);
        }

        [Fact]
        public void SetNullableShortTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (short?)5;

            Assert.Null(m.TestNullableShort);

            fda.Set(m, "TestNullableShort", testValue);

            Assert.Equal<short?>(testValue, m.TestNullableShort);
        }

        [Fact]
        public void SetNullableShort_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            short? testValue = null;

            Assert.Null(m.TestNullableShort);

            fda.Set(m, "TestNullableShort", testValue);

            Assert.Equal<short?>(testValue, m.TestNullableShort);
        }

        [Fact]
        public void SetNullableIntTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (int?)5;

            Assert.Null(m.TestNullableInt);

            fda.Set(m, "TestNullableInt", testValue);

            Assert.Equal<int?>(testValue, m.TestNullableInt);
        }

        [Fact]
        public void SetNullableint_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            int? testValue = null;

            Assert.Null(m.TestNullableInt);

            fda.Set(m, "TestNullableInt", testValue);

            Assert.Equal<int?>(testValue, m.TestNullableInt);
        }

        [Fact]
        public void SetNullableFloatTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (float?)5;

            Assert.Null(m.TestNullableFloat);

            fda.Set(m, "TestNullableFloat", testValue);

            Assert.Equal<float?>(testValue, m.TestNullableFloat);
        }

        [Fact]
        public void SetNullablefloat_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            float? testValue = null;

            Assert.Null(m.TestNullableFloat);

            fda.Set(m, "TestNullableFloat", testValue);

            Assert.Equal<float?>(testValue, m.TestNullableFloat);
        }

        [Fact]
        public void SetNullableDoubleTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (Double?)5;

            Assert.Null(m.TestNullableDouble);

            fda.Set(m, "TestNullableDouble", testValue);

            Assert.Equal<Double?>(testValue, m.TestNullableDouble);
        }

        [Fact]
        public void SetNullableDouble_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            Double? testValue = null;

            Assert.Null(m.TestNullableDouble);

            fda.Set(m, "TestNullableDouble", testValue);

            Assert.Equal<Double?>(testValue, m.TestNullableDouble);
        }

        [Fact]
        public void SetNullableDecimalTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (Decimal?)5;

            Assert.Null(m.TestNullableDecimal);

            fda.Set(m, "TestNullableDecimal", testValue);

            Assert.Equal<Decimal?>(testValue, m.TestNullableDecimal);
        }

        [Fact]
        public void SetNullableDecimal_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            Decimal? testValue = null;

            Assert.Null(m.TestNullableDecimal);

            fda.Set(m, "TestNullableDecimal", testValue);

            Assert.Equal<Decimal?>(testValue, m.TestNullableDecimal);
        }

        [Fact]
        public void SetNullableDateTimeTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (DateTime?)DateTime.Now;

            Assert.Null(m.TestNullableDateTime);

            fda.Set(m, "TestNullableDateTime", testValue);

            Assert.Equal<DateTime?>(testValue, m.TestNullableDateTime);
        }

        [Fact]
        public void SetNullableDateTime_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            DateTime? testValue = null;

            Assert.Null(m.TestNullableDateTime);

            fda.Set(m, "TestNullableDateTime", testValue);

            Assert.Equal<DateTime?>(testValue, m.TestNullableDateTime);
        }

        [Fact]
        public void SetNullableTimeSpanTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = (TimeSpan?)DateTime.Now.TimeOfDay;

            Assert.Null(m.TestNullableTimeSpan);

            fda.Set(m, "TestNullableTimeSpan", testValue);

            Assert.Equal<TimeSpan?>(testValue, m.TestNullableTimeSpan);
        }

        [Fact]
        public void SetNullableTimeSpan_Null_Test()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            TimeSpan? testValue = null;

            Assert.Null(m.TestNullableTimeSpan);

            fda.Set(m, "TestNullableTimeSpan", testValue);

            Assert.Equal<TimeSpan?>(testValue, m.TestNullableTimeSpan);
        }
        #endregion
    }
}