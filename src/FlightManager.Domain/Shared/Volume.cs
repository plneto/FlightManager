using System.Collections.Generic;
using FlightManager.Domain.Infrastructure;

namespace FlightManager.Domain.Shared
{
    public class Volume : ValueObject
    {
        public Volume(double litres)
        {
            Litres = litres;
        }

        public double Litres { get; }

        public double Gallons => Litres * 0.264;

        public static Volume FromLitres(double litres)
        {
            return new Volume(litres);
        }

        public static Volume FromGallons(double gallons)
        {
            return new Volume(gallons / 0.264);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Litres;
            yield return Gallons;
        }
    }
}