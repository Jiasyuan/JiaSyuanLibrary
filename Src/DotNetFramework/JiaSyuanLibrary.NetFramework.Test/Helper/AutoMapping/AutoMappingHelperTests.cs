using System;
using System.Collections.Generic;
using System.Linq;
using JiaSyuanLibrary.NetFramework.Helper.AutoMapping;
using Mapster;
using Xunit;

namespace JiaSyuanLibrary.NetFramework.Test.Helper.AutoMapping
{
    public class AutoMappingHelperTests
    {
        public class Source
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Destination
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public AutoMappingHelperTests()
        {
            // 每個測試前清空 MapperFactory 與 ProfileRegistry
            MapperFactory.ClearCache();
            foreach (var profile in MappingProfileRegistry.ListProfiles().ToList())
            {
                // 重新註冊 DefaultProfile 覆蓋
                MappingProfileRegistry.Register(profile, MappingProfileRegistry.DefaultProfile);
            }
        }

        [Fact]
        public void Map_Should_Map_Single_Object()
        {
            // Arrange
            MappingProfileRegistry.Register("default", cfg =>
            {
                cfg.NewConfig<Source, Destination>();
            });

            var source = new Source { Name = "JiaSyuan", Age = 30 };

            // Act
            var result = AutoMappingHelper.Map<Source, Destination>(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("JiaSyuan", result.Name);
            Assert.Equal(30, result.Age);
        }

        [Fact]
        public void MapCollection_Should_Map_List()
        {
            // Arrange
            MappingProfileRegistry.Register("default", cfg =>
            {
                cfg.NewConfig<Source, Destination>();
            });

            var sources = new List<Source>
            {
                new Source { Name = "A", Age = 1 },
                new Source { Name = "B", Age = 2 }
            };

            // Act
            var results = AutoMappingHelper.MapCollection<Source, Destination>(sources).ToList();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Contains(results, r => r.Name == "A" && r.Age == 1);
            Assert.Contains(results, r => r.Name == "B" && r.Age == 2);
        }

        [Fact]
        public void MapToExisting_Should_Update_Target()
        {
            // Arrange
            MappingProfileRegistry.Register("default", cfg =>
            {
                cfg.NewConfig<Source, Destination>();
            });

            var source = new Source { Name = "Updated", Age = 99 };
            var target = new Destination { Name = "Old", Age = 1 };

            // Act
            var result = AutoMappingHelper.MapToExisting(source, target);

            // Assert
            Assert.Equal("Updated", result.Name);
            Assert.Equal(99, result.Age);
        }

        [Fact]
        public void MapFromTwoSources_Should_Merge_Data()
        {
            // Arrange
            MappingProfileRegistry.Register("profile1", cfg =>
            {
                cfg.NewConfig<Source, Destination>();
            });
            MappingProfileRegistry.Register("profile2", cfg =>
            {
                cfg.NewConfig<Source, Destination>();
            });

            var source1 = new Source { Name = "First", Age = 10 };
            var source2 = new Source { Name = "Second", Age = 20 };

            // Act
            var result = AutoMappingHelper.MapFromTwoSources<Source, Source, Destination>(
                source1, source2, "profile1", "profile2");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Second", result.Name); // 第二次覆蓋
            Assert.Equal(20, result.Age);
        }

        [Fact]
        public void MapperFactory_Should_Cache_Mappers()
        {
            // Arrange
            var configAction = new Action<TypeAdapterConfig>(cfg =>
            {
                cfg.NewConfig<Source, Destination>();
            });

            // Act
            var mapper1 = MapperFactory.GetMapper(configAction, "cacheKey");
            var mapper2 = MapperFactory.GetMapper(configAction, "cacheKey");

            // Assert
            Assert.Same(mapper1, mapper2); // 同一快取實例
            Assert.True(MapperFactory.IsCached("cacheKey"));
        }

        [Fact]
        public void MappingProfileRegistry_Should_Register_And_Get_Profile()
        {
            // Arrange
            var called = false;
            MappingProfileRegistry.Register("testProfile", cfg => { called = true; });

            // Act
            var profile = MappingProfileRegistry.Get("testProfile");

            // 傳入 Mapster 的 TypeAdapterConfig 來觸發委派
            profile(new TypeAdapterConfig());

            // Assert
            Assert.True(called);
            Assert.True(MappingProfileRegistry.IsRegistered("testProfile"));
            Assert.Contains("testProfile", MappingProfileRegistry.ListProfiles());
        }

        [Fact]
        public void MappingProfileRegistry_Get_Should_Return_Default_When_Not_Found()
        {
            // Act
            var profile = MappingProfileRegistry.Get("notExist");

            // Assert
            Assert.NotNull(profile);

            var cfg = new TypeAdapterConfig();

            // 執行 DefaultProfile 委派，確保其能正常運作且不拋出異常
            var exception = Record.Exception(() => profile(cfg));
            Assert.Null(exception);

            // 驗證回傳的是預設配置（Mapster 的配置實例內部設定不為空）
            Assert.NotNull(cfg.Rules);
        }
    }

}
