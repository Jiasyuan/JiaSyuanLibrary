using AutoMapper;
using Moq;

namespace JiaSyuanLibrary.Net.Test.AutoMappingHelper
{
    public class AutoMappingHelperTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Net.AutoMappingHelper.AutoMappingHelper _helper;

        public AutoMappingHelperTests()
        {
            _mapperMock = new Mock<IMapper>();
            _helper = new Net.AutoMappingHelper.AutoMappingHelper(_mapperMock.Object);
        }

        [Fact]
        public void Map_ShouldReturnMappedObject_WhenSourceIsNotNull()
        {
            // Arrange
            var source = new Source { Value = "Test" };
            var expected = new Destination { Value = "Test" };

            _mapperMock.Setup(m => m.Map<Destination>(source)).Returns(expected);

            // Act
            var result = _helper.Map<Source, Destination>(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result?.Value);
        }

        [Fact]
        public void Map_ShouldReturnNull_WhenSourceIsNull()
        {
            // Act
            var result = _helper.Map<Source, Destination>(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MapCollection_ShouldReturnMappedCollection_WhenSourceIsValid()
        {
            // Arrange
            var sourceList = new List<Source>
        {
            new Source { Value = "A" },
            new Source { Value = "B" }
        };

            var expectedList = new List<Destination>
        {
            new Destination { Value = "A" },
            new Destination { Value = "B" }
        };

            _mapperMock.Setup(m => m.Map<IEnumerable<Destination>>(sourceList)).Returns(expectedList);

            // Act
            var result = _helper.MapCollection<Source, Destination>(sourceList);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Value == "A");
            Assert.Contains(result, r => r.Value == "B");
        }

        [Fact]
        public void MapCollection_ShouldReturnEmpty_WhenSourceIsNull()
        {
            // Act
            var result = _helper.MapCollection<Source, Destination>(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void MapToExisting_ShouldMapToTarget_WhenSourceIsNotNull()
        {
            // Arrange
            var source = new Source { Value = "Updated" };
            var target = new Destination { Value = "Original" };

            var expected = new Destination { Value = "Updated" };

            _mapperMock.Setup(m => m.Map(source, target)).Returns(expected);

            // Act
            var result = _helper.MapToExisting(source, target);

            // Assert
            Assert.Equal("Updated", result?.Value);
        }

        [Fact]
        public void MapToExisting_ShouldReturnTarget_WhenSourceIsNull()
        {
            // Arrange
            var target = new Destination { Value = "Original" };

            // Act
            var result = _helper.MapToExisting<Source, Destination>(null, target);

            // Assert
            Assert.Equal("Original", result?.Value);
        }

        // 測試用類別
        public class Source
        {
            public string Value { get; set; } = string.Empty;
        }

        public class Destination
        {
            public string Value { get; set; } = string.Empty;
        }
    }

}
