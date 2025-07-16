namespace XRoad.Core.Models;

public class XRoadServiceDescriptor
{
    public string Instance { get; set; } = string.Empty;
    public string MemberClass { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public string SubsystemCode { get; set; } = string.Empty;
    public string ServiceCode { get; set; } = string.Empty;
    public string ServiceVersion { get; set; } = string.Empty;

    public string GetFullServiceName() =>
        $"{Instance}/{MemberClass}/{MemberCode}/{SubsystemCode}/{ServiceCode}/{ServiceVersion}";
}