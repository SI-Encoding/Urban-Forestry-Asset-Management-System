using UFAMS.Application.Common.Exceptions;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Inspections.UpdateRecommendation;

public sealed class UpdateInspectionRecommendationHandler
{
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInspectionRecommendationHandler(
        IInspectionRepository inspectionRepository,
        IUnitOfWork unitOfWork)
    {
        _inspectionRepository = inspectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateInspectionRecommendationResponse> Handle(
        Guid id,
        UpdateInspectionRecommendationCommand command,
        CancellationToken cancellationToken = default)
    {
        var inspection =
            await _inspectionRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (inspection is null)
        {
            throw new NotFoundException(
                "Inspection",
                id);
        }

        inspection.UpdateRecommendation(
            command.Recommendation);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new UpdateInspectionRecommendationResponse(
            inspection.Id,
            inspection.TreeId,
            inspection.Recommendation);
    }
}