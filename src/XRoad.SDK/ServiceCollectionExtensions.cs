using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XRoad.Client;
using XRoad.Client.Serialization;
using XRoad.Core.Configuration;
using XRoad.Core.Interfaces;
using XRoad.Services.EMTA.TOR;
using XRoad.Services.EMTA.TOR.Interfaces;

namespace XRoad.SDK;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds X-Road services to the service collection with configuration
    /// </summary>
    public static IServiceCollection AddXRoadServices(
        this IServiceCollection services,
        IConfiguration configuration,
        string configSectionName = XRoadOptions.SectionName)
    {
        // Configure options
        services.Configure<XRoadOptions>(configuration.GetSection(configSectionName));

        // Add core services
        services.AddHttpClient();
        services.AddXRoadClient();
        services.AddXRoadTorServices();

        return services;
    }

    /// <summary>
    /// Adds X-Road services to the service collection with action-based configuration
    /// </summary>
    public static IServiceCollection AddXRoadServices(
        this IServiceCollection services,
        Action<XRoadOptions> configureOptions)
    {
        // Configure options
        services.Configure(configureOptions);

        // Add core services
        services.AddHttpClient();
        services.AddXRoadClient();
        services.AddXRoadTorServices();

        return services;
    }

    /// <summary>
    /// Adds X-Road client services
    /// </summary>
    public static IServiceCollection AddXRoadClient(this IServiceCollection services)
    {
        services.AddHttpClient("XRoadClient", (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<XRoadOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        })
        .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<XRoadOptions>>().Value;
            var handler = new HttpClientHandler();

            if (options.IgnoreSslErrors)
            {
                handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                {
                    // Log SSL validation bypass if needed
                    return true;
                };
            }

            // TODO: Add client certificate support if configured
            if (!string.IsNullOrEmpty(options.ClientCertificatePath))
            {
                // Load and attach client certificate
            }

            return handler;
        });

        services.AddScoped<IXRoadSerializer, XRoadSoapSerializer>();
        services.AddScoped<IXRoadClient, XRoadClient>();

        return services;
    }

    /// <summary>
    /// Adds TOR (Tax and Customs Board) services
    /// </summary>
    public static IServiceCollection AddXRoadTorServices(this IServiceCollection services)
    {
        services.AddScoped<ITorService, TorService>();
        return services;
    }

    /// <summary>
    /// Adds EMTA services (placeholder for future implementation)
    /// </summary>
    public static IServiceCollection AddXRoadEmtaServices(this IServiceCollection services)
    {
        // TODO: Implement EMTA services
        return services;
    }

    /// <summary>
    /// Adds SK ID Solutions services (placeholder for future implementation)
    /// </summary>
    public static IServiceCollection AddXRoadSkidServices(this IServiceCollection services)
    {
        // TODO: Implement SK ID services
        return services;
    }
}