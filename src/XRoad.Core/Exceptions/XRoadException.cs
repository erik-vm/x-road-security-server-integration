namespace XRoad.Core.Exceptions;

public class XRoadException : Exception
{
    public string? RequestId { get; }
    public string? ServiceName { get; }

    public XRoadException(string message) : base(message)
    {
    }

    public XRoadException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public XRoadException(string message, string? requestId, string? serviceName = null) : base(message)
    {
        RequestId = requestId;
        ServiceName = serviceName;
    }

    public XRoadException(string message, Exception innerException, string? requestId, string? serviceName = null)
        : base(message, innerException)
    {
        RequestId = requestId;
        ServiceName = serviceName;
    }
}

public class XRoadConfigurationException : XRoadException
{
    public XRoadConfigurationException(string message) : base(message)
    {
    }

    public XRoadConfigurationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class XRoadNetworkException : XRoadException
{
    public XRoadNetworkException(string message) : base(message)
    {
    }

    public XRoadNetworkException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public XRoadNetworkException(string message, string requestId) : base(message, requestId)
    {
    }
}

public class XRoadSoapException : XRoadException
{
    public string? SoapFaultCode { get; }
    public string? SoapFaultString { get; }

    public XRoadSoapException(string message, string? soapFaultCode = null, string? soapFaultString = null)
        : base(message)
    {
        SoapFaultCode = soapFaultCode;
        SoapFaultString = soapFaultString;
    }
}