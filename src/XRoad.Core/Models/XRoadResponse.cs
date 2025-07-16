namespace XRoad.Core.Models;

public class XRoadResponse<T>
{
    public string RequestId { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RawResponse { get; set; }
    public TimeSpan ProcessingTime { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static XRoadResponse<T> Success(string requestId, T data, TimeSpan processingTime,
        string? rawResponse = null)
        => new()
        {
            RequestId = requestId,
            IsSuccess = true,
            Data = data,
            ProcessingTime = processingTime,
            RawResponse = rawResponse
        };

    public static XRoadResponse<T> Failure(string requestId, string errorMessage, TimeSpan processingTime,
        string? rawResponse = null)
        => new()
        {
            RequestId = requestId,
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ProcessingTime = processingTime,
            RawResponse = rawResponse
        };
}