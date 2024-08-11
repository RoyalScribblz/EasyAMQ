using Apache.NMS;
using Apache.NMS.AMQP;
using EasyAMQ.Options;
using EasyAMQ.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyAMQ.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(
        this WebApplicationBuilder builder,
        Action<IServiceCollection, EasyAmqService> setup)
    {
        return builder.Services.Configure<EasyAmqOptions>(builder.Configuration.GetSection(nameof(EasyAmqOptions)))
            .AddSingleton<IConnectionFactory>(provider =>
                new ConnectionFactory(provider.GetRequiredService<IOptions<EasyAmqOptions>>().Value.ConnectionString))
            .AddHostedService(provider =>
            {
                var easyAmqService = new EasyAmqService(provider);

                setup(builder.Services, easyAmqService);

                return easyAmqService;
            });
    }
}