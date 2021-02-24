using System;
using System.Collections.Generic;
using System.Text;
using JiaSyuanLibrary.Standard.Helper;
using Xunit;

namespace UnitTest.Core.Standard.Helper
{
    public class MapperHelperTest
    {
        [Fact]
        public void MapperProperties()
        {
            var testModelC = new TestModelC()
            {
                PropertyString = "PropertyString",
                PropertyInt = 100,
                PropertyDateTime = DateTime.Now,
                PropertyDecimal = 1991.0504M
            };
            var testModelC_1 = MapperHelper.MapperProperties<TestModelC, TestModelC_1>(testModelC);
            Assert.Equal(testModelC.PropertyString, testModelC_1.PropertyString);
            Assert.Equal(testModelC.PropertyInt, testModelC_1.PropertyInt);
            Assert.Equal(testModelC.PropertyDecimal, testModelC_1.PropertyDecimal);
            Assert.Equal(testModelC.PropertyDecimal, testModelC_1.PropertyDecimal);
        }

        [Fact]
        public void MapperTwoClassPropertiesTest()
        {
            var testModelA = new TestModelA()
            {
                PropertyString = "PropertyString",
                PropertyInt =100
            };

            var testModelB = new TestModelB()
            {
                PropertyDateTime =DateTime.Now,
                PropertyDecimal = 1991.0504M
            };

            var testModelC = MapperHelper.MapperTwoClassProperties<TestModelA, TestModelB, TestModelC>(testModelA, testModelB);
            Assert.Equal(testModelC.PropertyString, testModelA.PropertyString);
            Assert.Equal(testModelC.PropertyInt, testModelA.PropertyInt);
            Assert.Equal(testModelC.PropertyDecimal, testModelB.PropertyDecimal);
            Assert.Equal(testModelC.PropertyDecimal, testModelB.PropertyDecimal);
        }

        [Fact]
        public void MapperPropertiesIntoDestination()
        {
            var testModelB = new TestModelB()
            {
                PropertyDateTime = DateTime.Now,
                PropertyDecimal = 1991.0504M
            };
            var testModelC = new TestModelC()
            {
                PropertyString = "PropertyString",
                PropertyInt = 100
            };
            testModelC = MapperHelper.MapperSourcePropertiesIntoDestination(testModelB, testModelC);
            Assert.Equal("PropertyString", testModelC.PropertyString);
            Assert.Equal(100, testModelC.PropertyInt);
            Assert.Equal(testModelB.PropertyDecimal, testModelC.PropertyDecimal);
            Assert.Equal(testModelB.PropertyDecimal, testModelC.PropertyDecimal);
        }
        
                [Fact]
        public void MapperPropertiesPascalCaseToUpperCase()
        {
            var testModelPascalCase = new TestModelPascalCase()
            {
                PropertyString = "PropertyString",
                PropertyInt = 100,
                PropertyDateTime = DateTime.Now,
                PropertyDecimal = 1991.0504M
            };

            TestModelUpperCase testModelUpperCase = MapperHelper.MapperPropertiesCaseInsensitive<TestModelPascalCase, TestModelUpperCase>(testModelPascalCase);
            Assert.Equal(testModelPascalCase.PropertyString, testModelUpperCase.PROPERTYSTRING);
            Assert.Equal(testModelPascalCase.PropertyInt, testModelUpperCase.PROPERTYINT);
            Assert.Equal(testModelPascalCase.PropertyDateTime, testModelUpperCase.PROPERTYDATETIME);
            Assert.Equal(testModelPascalCase.PropertyDecimal, testModelUpperCase.PROPERTYDECIMAL);
        }

        [Fact]
        public void MapperPropertiesUpperCaseToPascalCase()
        {
            var testModelUpperCase = new TestModelUpperCase()
            {
                PROPERTYSTRING = "PropertyString",
                PROPERTYINT = 100,
                PROPERTYDATETIME = DateTime.Now,
                PROPERTYDECIMAL = 1991.0504M
            };

            TestModelPascalCase testModelPascalCase = MapperHelper.MapperPropertiesCaseInsensitive<TestModelUpperCase, TestModelPascalCase>(testModelUpperCase);
            Assert.Equal(testModelPascalCase.PropertyString, testModelUpperCase.PROPERTYSTRING);
            Assert.Equal(testModelPascalCase.PropertyInt, testModelUpperCase.PROPERTYINT);
            Assert.Equal(testModelPascalCase.PropertyDateTime, testModelUpperCase.PROPERTYDATETIME);
            Assert.Equal(testModelPascalCase.PropertyDecimal, testModelUpperCase.PROPERTYDECIMAL);
        }
    }

    public class TestModelA
    {
        public string PropertyString { get; set; }
        public int PropertyInt { get; set; }
    }

    public class TestModelB
    {
        public DateTime PropertyDateTime { get; set; }
        public decimal PropertyDecimal { get; set; }
    }

    public class TestModelC
    {
        public string PropertyString { get; set; }
        public int PropertyInt { get; set; }
        public DateTime PropertyDateTime { get; set; }
        public decimal PropertyDecimal { get; set; }
    }

    public class TestModelC_1
    {
        public string PropertyString { get; set; }
        public int PropertyInt { get; set; }
        public DateTime PropertyDateTime { get; set; }
        public decimal PropertyDecimal { get; set; }
    }
    
    
    public class TestModelPascalCase
    {
        public string PropertyString { get; set; }
        public int PropertyInt { get; set; }
        public DateTime PropertyDateTime { get; set; }
        public decimal PropertyDecimal { get; set; }
    }

    public class TestModelUpperCase
    {
        public string PROPERTYSTRING { get; set; }
        public int PROPERTYINT { get; set; }
        public DateTime PROPERTYDATETIME { get; set; }
        public decimal PROPERTYDECIMAL { get; set; }
    }
}
