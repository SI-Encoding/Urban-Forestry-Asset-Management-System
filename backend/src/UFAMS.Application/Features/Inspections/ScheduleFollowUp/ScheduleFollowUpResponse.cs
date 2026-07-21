namespace UFAMS.Application.Features.Inspections.ScheduleFollowUp;

public sealed record ScheduleFollowUpResponse(
    Guid Id,
    Guid TreeId,
    DateOnly? NextInspectionDate);