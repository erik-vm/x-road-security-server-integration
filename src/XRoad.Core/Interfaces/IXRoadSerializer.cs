namespace XRoad.Core.Interfaces;

public interface IXRoadSerializer
{
    string SerializeRequest<T>(T request, string requestId, string? userId = null);
    TResponse DeserializeResponse<TResponse>(string responseXml);
    string CreateSoapEnvelope(string bodyContent, string requestId, string? userId = null);
}