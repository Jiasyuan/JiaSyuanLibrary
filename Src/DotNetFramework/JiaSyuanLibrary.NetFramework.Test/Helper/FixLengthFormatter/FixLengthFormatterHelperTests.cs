using System;
using JiaSyuanLibrary.NetFramework.Helper.FixLengthFormatter;
using JiaSyuanLibrary.NetFramework.Helper.FixLengthFormatter.Attribute;
using Xunit;

namespace JiaSyuanLibrary.NetFramework.Test.Helper.FixLengthFormatter
{
    public class FixLengthFormatterHelperTests
    {
        public class TestModel
        {
            [FixLength(Order = 1, Length = 5, IsPadLeft = false)]
            public string Name { get; set; }

            [FixLength(Order = 2, Length = 3, PadChar = '0')]
            public int Age { get; set; }

            [FixLength(Order = 3, Length = 8, DecimalPlaces = 2, PadChar = '0')]
            public decimal Amount { get; set; }

            [FixLength(Order = 4, Length = 19, DateTimeFormat = "yyyy/MM/dd HH:mm:ss")]
            public DateTime BirthDate { get; set; }

            [FixLength(Order = 5, Length = 1)]
            public bool IsActive { get; set; }
        }

        [Fact]
        public void Serialize_Should_Format_Fields_Correctly()
        {
            var model = new TestModel
            {
                Name = "Amy",
                Age = 7,
                Amount = 123.45m,
                BirthDate = new DateTime(2000, 1, 2, 3, 4, 5),
                IsActive = true
            };

            var result = FixLengthFormatterHelper.Serialize(model);

            // Name: "Amy  " (右補空白到5)
            Assert.StartsWith("Amy  ", result);
            // Age: "007"
            Assert.Contains("007", result);
            // Amount: "00012345" (去小數點後補0)
            Assert.Contains("00012345", result);
            // DateTime: "2000/01/02 03:04:05"
            Assert.Contains("2000/01/02 03:04:05", result);
            // Bool: "True" => "True" trimmed to length 1 => "T"
            Assert.EndsWith("T", result);
        }

        [Fact]
        public void Deserialize_Should_Parse_Fields_Correctly()
        {
            // 準備固定長度字串
            var fixedString =
                "Amy  " +   // Name
                "007" +     // Age
                "00012345" +// Amount (123.45)
                "2000/01/02 03:04:05" + // DateTime
                "Y";        // Bool (true)

            var model = FixLengthFormatterHelper.Deserialize<TestModel>(fixedString);

            Assert.Equal("Amy", model.Name);
            Assert.Equal(7, model.Age);
            Assert.Equal(123.45m, model.Amount);
            Assert.Equal(new DateTime(2000, 1, 2, 3, 4, 5), model.BirthDate);
            Assert.True(model.IsActive);
        }

        [Fact]
        public void Serialize_And_Deserialize_Should_Be_Reversible()
        {
            var original = new TestModel
            {
                Name = "小明", // 全形字測試
                Age = 42,
                Amount = 9.5m,
                BirthDate = new DateTime(2025, 9, 3, 21, 50, 0),
                IsActive = false
            };

            var serialized = FixLengthFormatterHelper.Serialize(original);
            var deserialized = FixLengthFormatterHelper.Deserialize<TestModel>(serialized);

            Assert.Equal(original.Name, deserialized.Name);
            Assert.Equal(original.Age, deserialized.Age);
            Assert.Equal(original.Amount, deserialized.Amount);
            Assert.Equal(original.BirthDate, deserialized.BirthDate);
            Assert.Equal(original.IsActive, deserialized.IsActive);
        }

        [Fact]
        public void Deserialize_Should_Throw_When_Length_Mismatch()
        {
            var badString = "TooShort";
            Assert.Throws<ArgumentException>(() =>
                FixLengthFormatterHelper.Deserialize<TestModel>(badString));
        }

        [Fact]
        public void Serialize_Should_Handle_Null_Values()
        {
            var model = new TestModel
            {
                Name = null,
                Age = 0,
                Amount = 0m,
                BirthDate = default,
                IsActive = false
            };

            var result = FixLengthFormatterHelper.Serialize(model);

            // Name 應該是5個空白
            Assert.StartsWith(new string(' ', 5), result);
        }
    }

}
