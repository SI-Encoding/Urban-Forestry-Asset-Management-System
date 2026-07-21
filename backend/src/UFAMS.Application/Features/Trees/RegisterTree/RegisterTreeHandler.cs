using UFAMS.Application.Interfaces;
using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Application.Common.Exceptions;
namespace UFAMS.Application.Features.Trees.RegisterTree;

public class RegisterTreeHandler
{
    private readonly ITreeRepository _treeRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IParkRepository _parkRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterTreeHandler(
        ITreeRepository treeRepository,
        ISpeciesRepository speciesRepository,
        IParkRepository parkRepository,
        IUnitOfWork unitOfWork)
    {
        _treeRepository = treeRepository;
        _speciesRepository = speciesRepository;
        _parkRepository = parkRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterTreeResponse> Handle(
        RegisterTreeCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validate the command.
        var validator = new RegisterTreeValidator();

        var errors = validator.Validate(command);

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }

        // Ensure the asset tag is unique.
        if (await _treeRepository.ExistsAsync(
                command.AssetTag,
                cancellationToken))
        {
            throw new ConflictException(
                $"Tree '{command.AssetTag}' already exists.");
        }

        // Load the Species.
        var species = await _speciesRepository.GetByIdAsync(
            command.SpeciesId,
            cancellationToken);

        if (species is null)
        {
            throw new NotFoundException(
                nameof(Species),
                command.SpeciesId);
        }

        // Load the Park.
        var park = await _parkRepository.GetByIdAsync(
            command.ParkId,
            cancellationToken);

        if (park is null)
        {
            throw new NotFoundException(
                nameof(Park),
                command.ParkId);
        }

        // Create the domain entity.
        var tree = new Tree(
            command.AssetTag,
            species,
            park,
            command.Location,
            TreeHealthStatus.Good,
            command.PlantingDate,
            command.HeightInMeters,
            command.DiameterInCentimeters);

        // Persist it.
        await _treeRepository.AddAsync(
            tree,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // Return the response.
        return new RegisterTreeResponse(
            tree.Id,
            tree.AssetTag,
            tree.Species.CommonName,
            tree.Park.Name,
            tree.Location,
            tree.HealthStatus);
    }
}