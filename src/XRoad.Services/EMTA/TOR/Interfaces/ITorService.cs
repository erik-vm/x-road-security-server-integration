using XRoad.Core.Models;

namespace XRoad.Services.EMTA.TOR.Interfaces;

public interface ITorService
{
    Task<XRoadResponse<string>> RegisterWorkAsync(
        string personalCode, 
        DateTime startDate, 
        decimal workTimeRatio = 1.0m, 
        string? userId = null,
        CancellationToken cancellationToken = default);

    Task<XRoadResponse<string>> FindWorkAsync(
        string employerCode, 
        string queryType = "A", 
        int fromRecord = 1, 
        string? userId = null,
        CancellationToken cancellationToken = default);

    Task<XRoadResponse<string>> TestConnectionAsync(
        string? userId = null,
        CancellationToken cancellationToken = default);
}