using System.Collections.Generic;
using FlightManager.Domain.Infrastructure;

namespace FlightManager.Domain.Shared
{
    public class Distance : ValueObject
    {
        public Distance(double meters)
        {
            Meters = meters;
        }

        public double Meters { get; }

        public double Kilometers => Meters / 1000.0;

        public static Distance FromMeters(double meters)
        {
            return new Distance(meters);
        }

        public static Distance FromKilometers(double kilometers)
        {
            return new Distance(kilometers * 1000.0);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Meters;
            yield return Kilometers;
        }
    }
}