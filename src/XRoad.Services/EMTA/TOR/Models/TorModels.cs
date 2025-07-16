namespace XRoad.Services.EMTA.TOR.Models;

public class RegisterWorkRequest
{
    public string PersonalCode { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public decimal WorkTimeRatio { get; set; } = 1.0m;
    public string? UserId { get; set; }
}

public class FindWorkRequest
{
    public string EmployerCode { get; set; } = string.Empty;
    public string QueryType { get; set; } = "A";
    public int FromRecord { get; set; } = 1;
    public string? UserId { get; set; }
}

public class WorkRegistrationResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public string? RegistrationId { get; set; }
}

public class WorkQueryResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<WorkRecord>? Records { get; set; }
}

public class WorkRecord
{
    public string? PersonalCode { get; set; }
    public string? EmployerCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? WorkTimeRatio { get; set; }
}