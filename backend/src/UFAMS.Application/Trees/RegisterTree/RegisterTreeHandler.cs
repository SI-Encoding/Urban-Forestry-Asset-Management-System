using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Trees.RegisterTree;

public class RegisterTreeHandler
{
    private readonly ITreeRepository _treeRepository;

    public RegisterTreeHandler(
        ITreeRepository treeRepository)
    {
        _treeRepository = treeRepository;
    }

    public Task<RegisterTreeResponse> Handle(
        RegisterTreeCommand command,
        CancellationToken cancellationToken = default)
    {
        var validator = new RegisterTreeValidator();

        var validationErrors = validator.Validate(command);

        if (validationErrors.Count > 0)
        {
            throw new ArgumentException(
                string.Join(Environment.NewLine, validationErrors),
                nameof(command));
        }

        // Verify the asset tag is unique.

        // Retrieve the Species.

        // Retrieve the Park.

        // Create the Tree domain entity.

        // Save the Tree.

        // Commit the transaction.

        // Return the registered Tree.

        return Task.FromException<RegisterTreeResponse>(
            new NotImplementedException());
    }
}