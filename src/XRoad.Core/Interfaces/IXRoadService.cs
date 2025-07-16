namespace XRoad.Core.Interfaces;

using XRoad.Core.Models;

public interface IXRoadService
{
    string ServiceName { get; }
    XRoadServiceDescriptor ServiceDescriptor { get; }
}