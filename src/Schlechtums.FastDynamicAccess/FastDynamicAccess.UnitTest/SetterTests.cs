using FastDynamicAccess.UnitTest.Types;
using System;
using Xunit;

namespace FastDynamicAccess.UnitTest
{
    public class SetterTests
    {
        [Fact]
        public void SetStringTest()
        {
            Type type = typeof(MyClass);
            var m = new MyClass();
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);
            var testValue = "Hello World";

            Assert.Null(m.TestString);

            fda.SetValue(m, "TestString", testValue);

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

            fda.SetValue(m, "TestBoolean", testValue);

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

            fda.SetValue(m, "TestByte", testValue);

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

            fda.SetValue(m, "TestInt", testValue);

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

            fda.SetValue(m, "TestFloat", testValue);

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

            fda.SetValue(m, "TestDouble", testValue);

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

            fda.SetValue(m, "TestDecimal", testValue);

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

            fda.SetValue(m, "TestDateTime", testValue);

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

            fda.SetValue(m, "TestTimeSpan", testValue);

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

            fda.SetValue(m, "TestByteArray", testValue);

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

            fda.SetValue(m, "TestByteArray", testValue);

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

            fda.SetValue(m, "TestNullableBoolean", testValue);

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

            fda.SetValue(m, "TestNullableBoolean", testValue);

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

            fda.SetValue(m, "TestNullableByte", testValue);

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

            fda.SetValue(m, "TestNullableByte", testValue);

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

            fda.SetValue(m, "TestNullableShort", testValue);

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

            fda.SetValue(m, "TestNullableShort", testValue);

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

            fda.SetValue(m, "TestNullableInt", testValue);

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

            fda.SetValue(m, "TestNullableInt", testValue);

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

            fda.SetValue(m, "TestNullableFloat", testValue);

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

            fda.SetValue(m, "TestNullableFloat", testValue);

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

            fda.SetValue(m, "TestNullableDouble", testValue);

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

            fda.SetValue(m, "TestNullableDouble", testValue);

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

            fda.SetValue(m, "TestNullableDecimal", testValue);

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

            fda.SetValue(m, "TestNullableDecimal", testValue);

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

            fda.SetValue(m, "TestNullableDateTime", testValue);

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

            fda.SetValue(m, "TestNullableDateTime", testValue);

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

            fda.SetValue(m, "TestNullableTimeSpan", testValue);

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

            fda.SetValue(m, "TestNullableTimeSpan", testValue);

            Assert.Equal<TimeSpan?>(testValue, m.TestNullableTimeSpan);
        }
    }
}