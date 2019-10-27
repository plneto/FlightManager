﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FlightManager.Application.DomainEventHandlers;
using FlightManager.Domain.AggregatesModel.ReportItemAggregate;
using FlightManager.Domain.Contracts.Repositories;
using FlightManager.Domain.Events;
using Moq;
using Xunit;

namespace FlightManager.Application.Tests.DomainEventHandlers
{
    public class FlightDeletedEventHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IReportItemRepository> _reportItemRepositoryMock;

        public FlightDeletedEventHandlerTests()
        {
            _fixture = new Fixture();
            _reportItemRepositoryMock = new Mock<IReportItemRepository>();
        }

        [Fact]
        public async Task FlightDeletedEventHandler_ValidEvent_Success()
        {
            // Arrange
            var expectedEvent = _fixture.Create<FlightDeletedEvent>();

            var target = new FlightDeletedEventHandler(_reportItemRepositoryMock.Object);

            // Act
            await target.Handle(expectedEvent, CancellationToken.None);

            // Assert
            _reportItemRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<ReportItem>()),
                Times.Once);
        }
    }
}