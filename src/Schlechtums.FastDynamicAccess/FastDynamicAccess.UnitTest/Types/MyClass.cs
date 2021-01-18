using System;
using System.Collections.Generic;
using System.Text;

namespace FastDynamicAccess.UnitTest.Types
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

        internal MySubClass SubClass { get; set; }
    }
}