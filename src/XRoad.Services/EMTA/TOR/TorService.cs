using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XRoad.Core.Configuration;
using XRoad.Core.Interfaces;
using XRoad.Core.Models;
using XRoad.Services.Common;
using XRoad.Services.EMTA.TOR.Interfaces;
using XRoad.Services.EMTA.TOR.Models;

namespace XRoad.Services.EMTA.TOR;

public class TorService : BaseXRoadService, ITorService
{
    private readonly XRoadOptions _options;

    public TorService(IXRoadClient client, IOptions<XRoadOptions> options, ILogger<TorService> logger)
        : base(client, logger)
    {
        _options = options.Value;
    }

    public override string ServiceName => "TOR (Tax and Customs Board)";
    public override XRoadServiceDescriptor ServiceDescriptor => TorServiceDescriptors.WorkRegistration;

    public async Task<XRoadResponse<string>> RegisterWorkAsync(
        string personalCode,
        DateTime startDate,
        decimal workTimeRatio = 1.0m,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registering work for person {PersonalCode}", personalCode);

        var soapBody = CreateWorkRegistrationSoapBody(personalCode, startDate, workTimeRatio);

        return await SendRawRequestAsync(
            soapBody,
            TorServiceDescriptors.WorkRegistration,
            userId,
            cancellationToken);
    }

    public async Task<XRoadResponse<string>> FindWorkAsync(
        string employerCode,
        string queryType = "A",
        int fromRecord = 1,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Finding work records for employer {EmployerCode}", employerCode);

        var soapBody = CreateWorkQuerySoapBody(employerCode, queryType, fromRecord);

        return await SendRawRequestAsync(
            soapBody,
            TorServiceDescriptors.WorkQuery,
            userId,
            cancellationToken);
    }

    public async Task<XRoadResponse<string>> TestConnectionAsync(
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Testing TOR service connection");

        var soapBody = $@"<tns:TORRGNO>
                <request>
                    <paringu_liik>A</paringu_liik>
                    <tvoim_id>{_options.ClientMemberCode}</tvoim_id>
                    <alates_kandest>1</alates_kandest>
                </request>
            </tns:TORRGNO>";

        return await SendRawRequestAsync(
            soapBody,
            TorServiceDescriptors.WorkQuery,
            userId ?? "48906090292",
            cancellationToken);
    }

    private string CreateWorkRegistrationSoapBody(string personalCode, DateTime startDate, decimal workTimeRatio)
    {
        return $@"<tns:TOOTREG>
                <request>
                    <isikukood>{personalCode}</isikukood>
                    <alg_kuup>{startDate:yyyy-MM-dd}</alg_kuup>
                    <tooa_koef>{workTimeRatio:F2}</tooa_koef>
                </request>
            </tns:TOOTREG>";
    }

    private string CreateWorkQuerySoapBody(string employerCode, string queryType, int fromRecord)
    {
        return $@"<tns:TORRGNO>
                <request>
                    <paringu_liik>{queryType}</paringu_liik>
                    <tvoim_id>{employerCode}</tvoim_id>
                    <alates_kandest>{fromRecord}</alates_kandest>
                </request>
            </tns:TORRGNO>";
    }
}