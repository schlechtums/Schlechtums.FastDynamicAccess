using FastDynamicAccess.UnitTest.Types;
using Schlechtums.FastDynamicAccess;
using System;
using Xunit;

namespace FastDynamicAccess.UnitTest
{
    public class ExtensionsTests
    {
        [Fact]
        public void GetValue_Test()
        {
            var expected = "Hello World";
            var m = new MyClass { TestString = expected };

            Assert.Equal(expected, (String)m.GetValue("TestString"));
        }

        [Fact]
        public void TryGetValue_Test()
        {
            Type type = typeof(MyClass);
            var expected = "Hello World";
            var m = new MyClass { TestString = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Assert.Equal(expected, (String)fda.TryGetValue(m, "TestString"));
            Assert.Null(fda.TryGetValue(m, "TestString2"));
        }

        [Fact]
        public void GetValueGeneric_Test()
        {
            var expected = "Hello World";
            var m = new MyClass { TestString = expected };

            Assert.Equal(expected, m.GetValue<String>("TestString"));
        }

        [Fact]
        public void TryGetValueGeneric_Test()
        {
            Type type = typeof(MyClass);
            var expected = "Hello World";
            var m = new MyClass { TestString = expected };
            var fda = Schlechtums.FastDynamicAccess.FastDynamicAccess.Get(type);

            Assert.Equal(expected, fda.TryGetValue<String>(m, "TestString"));
            Assert.Null(fda.TryGetValue<String>(m, "TestString2"));
        }

        [Fact]
        public void GetValueProperty_Test()
        {
            var name = "Ben Franklin";
            var age = 1000;
            var m = new MyClass
            {
                SubClass = new MySubClass
                { 
                    Name = name,
                    Age = age
                }
            };

            Assert.Equal(name, (String)m.GetValueAtPath("SubClass.Name"));
            Assert.Equal(age, (int)m.GetValueAtPath("SubClass.Age"));
        }

        [Fact]
        public void TryGetValueProperty_Test()
        {
            var name = "Ben Franklin";
            var age = 1000;
            var m = new MyClass
            {
                SubClass = new MySubClass
                {
                    Name = name,
                    Age = age
                }
            };

            Assert.Equal(name, (String)m.TryGetValueAtPath("SubClass.Name"));
            Assert.Equal(age, (int)m.TryGetValueAtPath("SubClass.Age"));

            m = new MyClass();
            Assert.Null((String)m.TryGetValueAtPath("SubClass.Name"));
            Assert.Null(m.TryGetValueAtPath("SubClass.Age"));
        }

        [Fact]
        public void GetValuePropertyGeneric_Test()
        {
            var name = "Ben Franklin";
            var age = 1000;
            var m = new MyClass
            {
                SubClass = new MySubClass
                {
                    Name = name,
                    Age = age
                }
            };

            Assert.Equal(name, m.GetValueAtPath<String>("SubClass.Name"));
            Assert.Equal(age, m.GetValueAtPath<int>("SubClass.Age"));
        }

        [Fact]
        public void TryGetValuePropertyGeneric_Test()
        {
            var name = "Ben Franklin";
            var age = 1000;
            var m = new MyClass
            {
                SubClass = new MySubClass
                {
                    Name = name,
                    Age = age
                }
            };

            Assert.Equal(name, m.TryGetValueAtPath<String>("SubClass.Name"));
            Assert.Equal(age, (int)m.TryGetValueAtPath<int>("SubClass.Age"));

            m = new MyClass();
            Assert.Null(m.TryGetValueAtPath<String>("SubClass.Name"));
            Assert.Null(m.TryGetValueAtPath("SubClass.Age"));
        }

        [Fact]
        public void SetValue_Test()
        {
            var m = new MyClass();
            var expected = "Hello World";

            m.SetValue(nameof(MyClass.TestString), expected);
            Assert.Equal(expected, m.TestString);
        }
    }
}