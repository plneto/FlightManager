using System;
using AutoFixture;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FluentAssertions;
using Xunit;

namespace FlightManager.Domain.Tests.AggregateModels.ReportItemAggregate
{
    public class ReportItemTests
    {
        private readonly Fixture _fixture;

        public ReportItemTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void InitializeReport_ValidParameters_Success()
        {
            // Arrange
            var message = _fixture.Create<string>();
            var messageData = _fixture.Create<object>();

            // Act
            var reportItem = new ReportItem(message, messageData);

            // Assert
            reportItem.Message.Should().BeEquivalentTo(message);
            reportItem.MessageData.Should().BeEquivalentTo(messageData);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void InitializeReport_EmptyOrNullMessage_ThrowsArgumentNullException(string message)
        {
            // Arrange
            var messageData = _fixture.Create<object>();

            // Act
            Action action = () => new ReportItem(message, messageData);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}