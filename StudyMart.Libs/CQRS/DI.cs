using Microsoft.Extensions.DependencyInjection;

namespace StudyMart.Libs.CQRS;

public static class DI
{
    public static IServiceCollection AddCQRS(this IServiceCollection services, Type type)
    {
        services.Scan(scan => scan.FromAssembliesOf(type)
        .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandler<,>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingCommandHandler<,>));

        return services;
    }
}
