using System;
using System.Collections.Generic;
using FlightManager.Domain.Infrastructure;

namespace FlightManager.Domain.AggregatesModel.AirportAggregate
{
    public class Coordinates : ValueObject
    {
        public Coordinates(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException(nameof(latitude));
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentException(nameof(longitude));
            }

            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}