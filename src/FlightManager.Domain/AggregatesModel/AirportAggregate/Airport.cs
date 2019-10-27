using System;
using FlightManager.Domain.Infrastructure;
using FlightManager.Domain.Shared;
using GeoCoordinatePortable;

namespace FlightManager.Domain.AggregatesModel.AirportAggregate
{
    public class Airport : Entity, IAggregateRoot
    {
        public Airport(string name, string code, double latitude, double longitude)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            Id = Guid.NewGuid();
            Name = name;
            Code = code;
            Coordinates = new Coordinates(latitude, longitude);
            CreatedAt = DateTimeOffset.Now;
        }

        public string Name { get; private set; }

        public string Code { get; private set; }

        public Coordinates Coordinates { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public Distance GetDistanceTo(Airport destinationAirport)
        {
            var departure = new GeoCoordinate(
                this.Coordinates.Latitude,
                this.Coordinates.Longitude);

            var destination = new GeoCoordinate(
                destinationAirport.Coordinates.Latitude,
                destinationAirport.Coordinates.Longitude);

            var distanceInMeters = departure.GetDistanceTo(destination);

            return Distance.FromMeters(distanceInMeters);
        }

        public override string ToString()
        {
            return $"{Name} ({Code})";
        }
    }
}