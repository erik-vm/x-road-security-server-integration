using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XRoad.Core.Configuration;
using XRoad.Core.Exceptions;
using XRoad.Core.Interfaces;
using XRoad.Core.Models;

namespace XRoad.Client;

public class XRoadClient : IXRoadClient
{
    private readonly HttpClient _httpClient;
    private readonly XRoadOptions _options;
    private readonly IXRoadSerializer _serializer;
    private readonly ILogger<XRoadClient> _logger;

    public XRoadClient(
        IHttpClientFactory httpClientFactory,
        IOptions<XRoadOptions> options,
        IXRoadSerializer serializer,
        ILogger<XRoadClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("XRoadClient");
        _options = options.Value;
        _serializer = serializer;
        _logger = logger;

        _logger.LogInformation("XRoad Client initialized with server: {ServerUrl}", _options.SecurityServerUrl);
    }

    public async Task<XRoadResponse<TResponse>> SendAsync<TRequest, TResponse>(
        XRoadRequest<TRequest> request,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = request.RequestId ?? GenerateRequestId();

        try
        {
            _logger.LogInformation("Sending X-Road request {RequestId} to service {ServiceName}",
                requestId, request.Service.GetFullServiceName());

            var soapRequest = _serializer.SerializeRequest(request.RequestData, requestId, request.UserId);
            var responseXml = await SendSoapRequestAsync(soapRequest, cancellationToken);

            var responseData = _serializer.DeserializeResponse<TResponse>(responseXml);

            stopwatch.Stop();
            _logger.LogInformation("X-Road request {RequestId} completed successfully in {ElapsedMs}ms",
                requestId, stopwatch.ElapsedMilliseconds);

            return XRoadResponse<TResponse>.Success(requestId, responseData, stopwatch.Elapsed, responseXml);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "X-Road request {RequestId} failed after {ElapsedMs}ms",
                requestId, stopwatch.ElapsedMilliseconds);

            return XRoadResponse<TResponse>.Failure(requestId, ex.Message, stopwatch.Elapsed);
        }
    }

    public async Task<XRoadResponse<string>> SendRawAsync<TRequest>(
        XRoadRequest<TRequest> request,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = request.RequestId ?? GenerateRequestId();

        try
        {
            _logger.LogInformation("Sending raw X-Road request {RequestId} to service {ServiceName}",
                requestId, request.Service.GetFullServiceName());

            var soapRequest = _serializer.SerializeRequest(request.RequestData, requestId, request.UserId);
            var responseXml = await SendSoapRequestAsync(soapRequest, cancellationToken);

            stopwatch.Stop();
            return XRoadResponse<string>.Success(requestId, responseXml, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Raw X-Road request {RequestId} failed after {ElapsedMs}ms",
                requestId, stopwatch.ElapsedMilliseconds);

            return XRoadResponse<string>.Failure(requestId, ex.Message, stopwatch.Elapsed);
        }
    }

    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Testing X-Road connection to {ServerUrl}", _options.SecurityServerUrl);

            var testRequest = CreateTestRequest();
            var content = new StringContent(testRequest, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "");

            var response = await _httpClient.PostAsync(_options.SecurityServerUrl, content, cancellationToken);
            var isSuccess = response.IsSuccessStatusCode;

            _logger.LogInformation("X-Road connection test result: {Result}", isSuccess ? "Success" : "Failed");
            return isSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "X-Road connection test failed");
            return false;
        }
    }

    private async Task<string> SendSoapRequestAsync(string soapRequest, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Sending SOAP request to: {Url}", _options.SecurityServerUrl);

            var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "");

            var response = await _httpClient.PostAsync(_options.SecurityServerUrl, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("HTTP Error: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                throw new XRoadNetworkException($"Request failed with status {response.StatusCode}: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Received response with length: {Length}", responseContent.Length);

            return responseContent;
        }
        catch (HttpRequestException httpEx)
        {
            throw new XRoadNetworkException("HTTP request failed", httpEx);
        }
        catch (TaskCanceledException tcEx)
        {
            throw new XRoadNetworkException("Request timed out", tcEx);
        }
        catch (Exception ex) when (!(ex is XRoadException))
        {
            throw new XRoadException("Unexpected error during SOAP request", ex);
        }
    }

    private string CreateTestRequest()
    {
        var requestId = GenerateRequestId();
        return _serializer.CreateSoapEnvelope("<test>ping</test>", requestId);
    }

    private string GenerateRequestId()
    {
        return $"req-{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..8]}";
    }
}