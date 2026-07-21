namespace UFAMS.Application.Features.Inspections.ScheduleFollowUp;

public sealed record ScheduleFollowUpCommand(
    DateOnly? NextInspectionDate);