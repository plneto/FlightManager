using System.Collections.Generic;
using FlightManager.Domain.AggregatesModel.AirportAggregate;
using MongoDB.Driver;

namespace FlightManager.Web.SampleData
{
    public static class AirportSeedExtensions
    {
        public static IMongoDatabase AirportSeed(this IMongoDatabase context)
        {
            var collection = context.GetCollection<Airport>(nameof(Airport));
            var hasData = collection.Find(c => true).Any();

            if (!hasData)
            {
                var data = GetData();
                collection.InsertMany(data);
            }

            return context;
        }

        private static IEnumerable<Airport> GetData()
        {
            return new List<Airport>
            {
                new Airport("Francisco Sá Carneiro Airport", "OPO", 41.241662, -8.678566),
                new Airport("Heathrow Airport", "LHR", 51.470166, -0.454426),
                new Airport("Frankfurt Airport", "FRA", 50.038374, 8.562130),
                new Airport("Paris-Charles De Gaulle", "CDG", 49.009367, 2.548366),
                new Airport("São Paulo International Airport", "GRU", -23.431545, -46.473545),
                new Airport("JFK Airport", "JFK", 40.641832, -73.779083),
                new Airport("Narita International Airport", "NRT", 35.772674, 140.392292),
                new Airport("Sydney Airport", "SYD", -33.939860, 151.175791),
                new Airport("Brisbane Airport", "BNE", -27.394167, 153.122592),
                new Airport("Johannesburg Airport", "JNB", -26.136885, 28.242455),
            };
        }
    }
}