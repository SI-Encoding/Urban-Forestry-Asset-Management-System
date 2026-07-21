using UFAMS.Domain.Common;
using UFAMS.Domain.Enums;

namespace UFAMS.Domain.Entities;

public class Inspection : BaseEntity
{
    public Guid TreeId { get; private set; }

    public Tree Tree { get; private set; } = null!;

    public DateOnly InspectionDate { get; private set; }

    public TreeHealthStatus ObservedHealth { get; private set; }

    public string Notes { get; private set; }

    public string Recommendation { get; private set; } = string.Empty;

    public DateOnly? NextInspectionDate { get; private set; }

    private Inspection()
    {
        Tree = null!;
        Notes = null!;
        Recommendation = null!;
    }

    public Inspection(
        Guid treeId,
        DateOnly inspectionDate,
        TreeHealthStatus observedHealth,
        string notes,
        string recommendation,
        DateOnly? nextInspectionDate)
    {
        TreeId = treeId;

        InspectionDate = ValidateInspectionDate(
            inspectionDate);

        ObservedHealth = observedHealth;

        Notes = ValidateNotes(notes);

        Recommendation = ValidateRecommendation(
            recommendation);

        NextInspectionDate = ValidateNextInspectionDate(
            inspectionDate,
            nextInspectionDate);
    }

    public void UpdateNotes(
        string notes)
    {
        Notes = ValidateNotes(notes);

        MarkUpdated();
    }

    public void UpdateRecommendation(
        string recommendation)
    {
        Recommendation = ValidateRecommendation(
            recommendation);

        MarkUpdated();
    }

    public void ScheduleFollowUp(
        DateOnly? nextInspectionDate)
    {
        NextInspectionDate = ValidateNextInspectionDate(
            InspectionDate,
            nextInspectionDate);

        MarkUpdated();
    }

    private static DateOnly ValidateInspectionDate(
        DateOnly inspectionDate)
    {
        if (inspectionDate > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Inspection date cannot be in the future.",
                nameof(inspectionDate));

        return inspectionDate;
    }

    private static string ValidateNotes(
        string notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
            throw new ArgumentException(
                "Notes are required.",
                nameof(notes));

        return notes.Trim();
    }

    private static string ValidateRecommendation(
        string recommendation)
    {
        if (string.IsNullOrWhiteSpace(recommendation))
            throw new ArgumentException(
                "Recommendation is required.",
                nameof(recommendation));

        return recommendation.Trim();
    }

    private static DateOnly? ValidateNextInspectionDate(
        DateOnly inspectionDate,
        DateOnly? nextInspectionDate)
    {
        if (nextInspectionDate.HasValue &&
            nextInspectionDate.Value < inspectionDate)
        {
            throw new ArgumentException(
                "Next inspection date cannot be before the inspection date.",
                nameof(nextInspectionDate));
        }

        return nextInspectionDate;
    }
}