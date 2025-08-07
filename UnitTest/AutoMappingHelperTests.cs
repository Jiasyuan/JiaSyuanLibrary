using JiaSyuanLibrary.Helper.AutoMapping;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    public class AutoMappingHelperTests
    {
        class A { public string Name { get; set; } }
        class B { public string Name { get; set; } }

        [SetUp]
        public void Setup()
        {
            MappingProfileRegistry.Register("default", cfg =>
            {
                cfg.CreateMap<A, B>();
            });

            MapperFactory.ClearCache();
        }

        [Test]
        public void Map_SingleObject_ShouldMapCorrectly()
        {
            var a = new A { Name = "Test" };
            var b = AutoMappingHelper.Map<A, B>(a);
            Assert.IsNotNull(b);
            Assert.AreEqual("Test", b.Name);
        }

        [Test]
        public void MapCollection_ShouldMapAllItems()
        {
            var list = new List<A>
        {
            new A { Name = "Alice" },
            new A { Name = "Bob" }
        };

            var result = AutoMappingHelper.MapCollection<A, B>(list);
            Assert.AreEqual(2, new List<B>(result).Count);
            CollectionAssert.AreEqual(new[] { "Alice", "Bob" }, new List<B>(result).ConvertAll(x => x.Name));
        }

        [Test]
        public void MapToExisting_ShouldUpdateTargetObject()
        {
            var source = new A { Name = "Updated" };
            var target = new B { Name = "Old" };

            var result = AutoMappingHelper.MapToExisting(source, target);
            Assert.AreEqual("Updated", result.Name);
        }

        [Test]
        public void MapFromTwoSources_ShouldMergeTwoSources()
        {
        class C { public string Age { get; set; } }
        class D { public string Name { get; set; } public string Age { get; set; } }

        MappingProfileRegistry.Register("profileA", cfg => cfg.CreateMap<A, D>());
        MappingProfileRegistry.Register("profileC", cfg => cfg.CreateMap<C, D>());

        var a = new A { Name = "John" };
        var c = new C { Age = "42" };
        var merged = AutoMappingHelper.MapFromTwoSources<A, C, D>(a, c, "profileA", "profileC");

        Assert.AreEqual("John", merged.Name);
        Assert.AreEqual("42", merged.Age);
    }

    [Test]
        public void MapperFactory_Cache_ShouldReuseMapperInstance()
        {
            var mapper1 = MapperFactory.GetMapper(cfg => cfg.CreateMap<A, B>(), "shared");
            var mapper2 = MapperFactory.GetMapper(cfg => cfg.CreateMap<A, B>(), "shared");

            Assert.AreSame(mapper1, mapper2);
        }

        [Test]
        public void MappingProfileRegistry_Fallback_ShouldUseDefault()
        {
            var defaultAction = MappingProfileRegistry.Get("nonexistent");
            Assert.IsNotNull(defaultAction);
        }
    }

}
