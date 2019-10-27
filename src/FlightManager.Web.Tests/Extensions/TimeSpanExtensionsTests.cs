using System;
using FlightManager.Web.Extensions;
using FluentAssertions;
using Xunit;

namespace FlightManager.Web.Tests.Extensions
{
    public class TimeSpanExtensionsTests
    {
        [Fact]
        public void ToTotalHoursAndMinutesFormat_ValidTimeSpanMinutesLowerThanNine_ValidOutput()
        {
            // Arrange
            const int hours = 30;
            const int minutes = 9;
            var timeSpan = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

            // Act
            var result = timeSpan.ToTotalHoursAndMinutesFormat();

            // Assert
            result.Should().BeEquivalentTo($"{hours}:0{minutes}");
        }

        [Fact]
        public void ToTotalHoursAndMinutesFormat_ValidTimeSpanMinutesGreaterThanNine_ValidOutput()
        {
            // Arrange
            const int hours = 30;
            const int minutes = 15;
            var timeSpan = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

            // Act
            var result = timeSpan.ToTotalHoursAndMinutesFormat();

            // Assert
            result.Should().BeEquivalentTo($"{hours}:{minutes}");
        }
    }
}