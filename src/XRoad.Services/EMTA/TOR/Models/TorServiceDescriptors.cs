using XRoad.Core.Models;

namespace XRoad.Services.EMTA.TOR.Models;

public static class TorServiceDescriptors
{
    private const string TorInstance = "ee-test";
    private const string TorMemberClass = "GOV";
    private const string TorMemberCode = "70000349";
    private const string TorSubsystemCode = "tor";
    private const string TorVersion = "v2";

    public static XRoadServiceDescriptor WorkRegistration => new()
    {
        Instance = TorInstance,
        MemberClass = TorMemberClass,
        MemberCode = TorMemberCode,
        SubsystemCode = TorSubsystemCode,
        ServiceCode = "TOOTREG",
        ServiceVersion = TorVersion
    };

    public static XRoadServiceDescriptor WorkQuery => new()
    {
        Instance = TorInstance,
        MemberClass = TorMemberClass,
        MemberCode = TorMemberCode,
        SubsystemCode = TorSubsystemCode,
        ServiceCode = "TORRGNO",
        ServiceVersion = TorVersion
    };
}