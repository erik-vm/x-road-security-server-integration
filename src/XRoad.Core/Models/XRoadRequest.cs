namespace XRoad.Core.Models;

public class XRoadRequest<T>
{
    public XRoadServiceDescriptor Service { get; set; } = new();
    public string? UserId { get; set; }
    public T RequestData { get; set; } = default!;
    public string? RequestId { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
}