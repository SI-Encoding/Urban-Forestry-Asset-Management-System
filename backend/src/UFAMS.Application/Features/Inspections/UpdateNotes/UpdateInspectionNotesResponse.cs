namespace UFAMS.Application.Features.Inspections.UpdateNotes;

public sealed record UpdateInspectionNotesResponse(
    Guid Id,
    Guid TreeId,
    string Notes);