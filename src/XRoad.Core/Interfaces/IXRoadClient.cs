namespace XRoad.Core.Interfaces;

using XRoad.Core.Models;

public interface IXRoadClient
{
    Task<XRoadResponse<TResponse>> SendAsync<TRequest, TResponse>(
        XRoadRequest<TRequest> request,
        CancellationToken cancellationToken = default);

    Task<XRoadResponse<string>> SendRawAsync<TRequest>(
        XRoadRequest<TRequest> request,
        CancellationToken cancellationToken = default);

    Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);
}