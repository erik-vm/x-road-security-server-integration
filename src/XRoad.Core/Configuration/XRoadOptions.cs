using System.ComponentModel.DataAnnotations;

namespace XRoad.Core.Configuration;

public class XRoadOptions
{
    public const string SectionName = "XRoad";

    [Required]
    public string SecurityServerUrl { get; set; } = string.Empty;

    [Required]
    public string ClientInstance { get; set; } = string.Empty;

    [Required]
    public string ClientMemberClass { get; set; } = string.Empty;

    [Required]
    public string ClientMemberCode { get; set; } = string.Empty;

    [Required]
    public string ClientSubsystemCode { get; set; } = string.Empty;

    public string ProtocolVersion { get; set; } = "4.0";
    public bool IgnoreSslErrors { get; set; } = false;
    public string? ClientCertificatePath { get; set; }
    public string? ClientCertificatePassword { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
    public string Environment { get; set; } = "test"; // test, production
}