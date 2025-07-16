using System.Text;
using Microsoft.Extensions.Options;
using XRoad.Core.Configuration;
using XRoad.Core.Interfaces;
using XRoad.Core.Models;

namespace XRoad.Client.Serialization;

public class XRoadSoapSerializer : IXRoadSerializer
{
    private readonly XRoadOptions _options;

    public XRoadSoapSerializer(IOptions<XRoadOptions> options)
    {
        _options = options.Value;
    }

    public string SerializeRequest<T>(T request, string requestId, string? userId = null)
    {
        // For now, return the request as-is if it's already a string (soap body)
        // In a real implementation, you'd serialize the object to XML
        if (request is string soapBody)
        {
            return CreateSoapEnvelope(soapBody, requestId, userId);
        }

        // TODO: Implement proper object-to-XML serialization
        throw new NotImplementedException("Object serialization not yet implemented");
    }

    public TResponse DeserializeResponse<TResponse>(string responseXml)
    {
        // For now, return the raw XML if TResponse is string
        if (typeof(TResponse) == typeof(string))
        {
            return (TResponse)(object)responseXml;
        }

        // TODO: Implement proper XML-to-object deserialization
        throw new NotImplementedException("Object deserialization not yet implemented");
    }

    public string CreateSoapEnvelope(string bodyContent, string requestId, string? userId = null)
    {
        var envelope = new StringBuilder();
        envelope.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        envelope.AppendLine("<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"");
        envelope.AppendLine("                   xmlns:id=\"http://x-road.eu/xsd/identifiers\"");
        envelope.AppendLine("                   xmlns:xroad=\"http://x-road.eu/xsd/xroad.xsd\">");
        envelope.AppendLine("    <SOAP-ENV:Header>");
        envelope.AppendLine("        <xroad:client id:objectType=\"SUBSYSTEM\">");
        envelope.AppendLine($"            <id:xRoadInstance>{_options.ClientInstance}</id:xRoadInstance>");
        envelope.AppendLine($"            <id:memberClass>{_options.ClientMemberClass}</id:memberClass>");
        envelope.AppendLine($"            <id:memberCode>{_options.ClientMemberCode}</id:memberCode>");
        envelope.AppendLine($"            <id:subsystemCode>{_options.ClientSubsystemCode}</id:subsystemCode>");
        envelope.AppendLine("        </xroad:client>");
        envelope.AppendLine($"        <xroad:id>{requestId}</xroad:id>");
        envelope.AppendLine($"        <xroad:protocolVersion>{_options.ProtocolVersion}</xroad:protocolVersion>");
        
        if (!string.IsNullOrEmpty(userId))
        {
            envelope.AppendLine($"        <xroad:userId>{userId}</xroad:userId>");
        }
        
        envelope.AppendLine("    </SOAP-ENV:Header>");
        envelope.AppendLine("    <SOAP-ENV:Body>");
        envelope.AppendLine($"        {bodyContent}");
        envelope.AppendLine("    </SOAP-ENV:Body>");
        envelope.AppendLine("</SOAP-ENV:Envelope>");

        return envelope.ToString();
    }

    public string CreateServiceSpecificSoapEnvelope(XRoadServiceDescriptor service, string bodyContent, string requestId, string? userId = null)
    {
        var envelope = new StringBuilder();
        envelope.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        envelope.AppendLine("<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"");
        envelope.AppendLine("                   xmlns:id=\"http://x-road.eu/xsd/identifiers\"");
        envelope.AppendLine("                   xmlns:tns=\"http://emta-v6.x-road.eu\"");
        envelope.AppendLine("                   xmlns:xroad=\"http://x-road.eu/xsd/xroad.xsd\">");
        envelope.AppendLine("    <SOAP-ENV:Header>");
        envelope.AppendLine("        <xroad:client id:objectType=\"SUBSYSTEM\">");
        envelope.AppendLine($"            <id:xRoadInstance>{_options.ClientInstance}</id:xRoadInstance>");
        envelope.AppendLine($"            <id:memberClass>{_options.ClientMemberClass}</id:memberClass>");
        envelope.AppendLine($"            <id:memberCode>{_options.ClientMemberCode}</id:memberCode>");
        envelope.AppendLine($"            <id:subsystemCode>{_options.ClientSubsystemCode}</id:subsystemCode>");
        envelope.AppendLine("        </xroad:client>");
        envelope.AppendLine("        <xroad:service id:objectType=\"SERVICE\">");
        envelope.AppendLine($"            <id:xRoadInstance>{service.Instance}</id:xRoadInstance>");
        envelope.AppendLine($"            <id:memberClass>{service.MemberClass}</id:memberClass>");
        envelope.AppendLine($"            <id:memberCode>{service.MemberCode}</id:memberCode>");
        envelope.AppendLine($"            <id:subsystemCode>{service.SubsystemCode}</id:subsystemCode>");
        envelope.AppendLine($"            <id:serviceCode>{service.ServiceCode}</id:serviceCode>");
        envelope.AppendLine($"            <id:serviceVersion>{service.ServiceVersion}</id:serviceVersion>");
        envelope.AppendLine("        </xroad:service>");
        envelope.AppendLine($"        <xroad:id>{requestId}</xroad:id>");
        envelope.AppendLine($"        <xroad:protocolVersion>{_options.ProtocolVersion}</xroad:protocolVersion>");
        
        if (!string.IsNullOrEmpty(userId))
        {
            envelope.AppendLine($"        <xroad:userId>{userId}</xroad:userId>");
        }
        
        envelope.AppendLine("    </SOAP-ENV:Header>");
        envelope.AppendLine("    <SOAP-ENV:Body>");
        envelope.AppendLine($"        {bodyContent}");
        envelope.AppendLine("    </SOAP-ENV:Body>");
        envelope.AppendLine("</SOAP-ENV:Envelope>");

        return envelope.ToString();
    }
}