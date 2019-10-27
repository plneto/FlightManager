using FlightManager.Web.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FlightManager.Web.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddMongoDb(
            this IServiceCollection services,
            MongoSettings mongoSettings)
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
            services.AddSingleton<IMongoDatabase>(serviceProvider =>
            {
                var client = serviceProvider.GetService<IMongoClient>();

                return client.GetDatabase(mongoSettings.DatabaseName);
            });

            return services;
        }
    }
}