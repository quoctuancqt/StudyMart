using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StudyMart.AppHost;

public static class EventExtensions
{
    internal static void SubscribeAppHostEvent(this IDistributedApplicationBuilder builder)
    {
        builder.Eventing.Subscribe<BeforeStartEvent>(
            static (@event, cancellationToken) =>
            {
                var logger = @event.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("1. BeforeStartEvent");

                return Task.CompletedTask;
            });

        builder.Eventing.Subscribe<AfterEndpointsAllocatedEvent>(
            static (@event, cancellationToken) =>
            {
                var logger = @event.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("2. AfterEndpointsAllocatedEvent");

                return Task.CompletedTask;
            });

        builder.Eventing.Subscribe<AfterResourcesCreatedEvent>(
            static (@event, cancellationToken) =>
            {
                var logger = @event.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("3. AfterResourcesCreatedEvent");

                return Task.CompletedTask;
            });
    }

    internal static void SubscribeResourceEvent(this IDistributedApplicationBuilder builder, IResource resource)
    {
        builder.Eventing.Subscribe<ResourceReadyEvent>(
            resource,
            static (@event, cancellationToken) =>
            {
                var logger = @event.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("3. ResourceReadyEvent");

                return Task.CompletedTask;
            });

        builder.Eventing.Subscribe<BeforeResourceStartedEvent>(
            resource,
            static (@event, cancellationToken) =>
            {
                var logger = @event.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("2. BeforeResourceStartedEvent");

                return Task.CompletedTask;
            });

        builder.Eventing.Subscribe<ConnectionStringAvailableEvent>(
            resource,
            static (@event, cancellationToken) =>
            {
                var logger = @event.Services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("1. ConnectionStringAvailableEvent");

                return Task.CompletedTask;
            });
    }
}