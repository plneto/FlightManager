using System;
using AutoFixture;
using FlightManager.Common.Extensions;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FlightManager.Common.Tests.Extensions
{
    public class JsonExtensionsTests
    {
        private readonly Fixture _fixture;

        public JsonExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ToJson_ValidObject_ReturnsValidJson()
        {
            // Arrange
            var reportItem = _fixture.Create<ReportItem>();

            // Act
            Action action = () => JObject.Parse(reportItem.ToJson());

            // Assert
            action.Should().NotThrow();
        }
    }
}