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

        public static Volume FromLitres(double litres)
        {
            return new Volume(litres);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Litres;
        }
    }
}