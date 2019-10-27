using System.Linq;
using System.Threading.Tasks;
using FlightManager.Domain.Infrastructure;
using MediatR;

namespace FlightManager.Domain.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, Entity entity)
        {
            var domainEvents = entity.DomainEvents.ToList();

            entity.ClearDomainEvents();

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}