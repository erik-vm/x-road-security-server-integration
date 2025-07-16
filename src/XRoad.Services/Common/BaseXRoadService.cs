using Microsoft.Extensions.Logging;
using XRoad.Core.Interfaces;
using XRoad.Core.Models;

namespace XRoad.Services.Common;

public abstract class BaseXRoadService : IXRoadService
{
    protected readonly IXRoadClient _client;
    protected readonly ILogger _logger;

    protected BaseXRoadService(IXRoadClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }

    public abstract string ServiceName { get; }
    public abstract XRoadServiceDescriptor ServiceDescriptor { get; }

    protected async Task<XRoadResponse<TResponse>> SendRequestAsync<TRequest, TResponse>(
        TRequest requestData,
        XRoadServiceDescriptor? serviceOverride = null,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var service = serviceOverride ?? ServiceDescriptor;
        var request = new XRoadRequest<TRequest>
        {
            Service = service,
            RequestData = requestData,
            UserId = userId
        };

        return await _client.SendAsync<TRequest, TResponse>(request, cancellationToken);
    }

    protected async Task<XRoadResponse<string>> SendRawRequestAsync<TRequest>(
        TRequest requestData,
        XRoadServiceDescriptor? serviceOverride = null,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var service = serviceOverride ?? ServiceDescriptor;
        var request = new XRoadRequest<TRequest>
        {
            Service = service,
            RequestData = requestData,
            UserId = userId
        };

        return await _client.SendRawAsync(request, cancellationToken);
    }
}