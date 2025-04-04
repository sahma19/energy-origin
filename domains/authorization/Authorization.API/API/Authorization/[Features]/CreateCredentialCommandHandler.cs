using System;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using MediatR;

namespace API.Authorization._Features_;

public class CreateCredentialCommandHandler(ICredentialService credentialService)
    : IRequestHandler<CreateCredentialCommand, CreateCredentialCommandResult>
{
    public async Task<CreateCredentialCommandResult> Handle(CreateCredentialCommand command,
        CancellationToken cancellationToken)
    {
        var credential = await credentialService.CreateCredential(command.ApplicationId, cancellationToken);
        return new CreateCredentialCommandResult(credential.Hint, credential.KeyId, credential.StartDateTime,
            credential.EndDateTime, credential.Secret);
    }
}

public record CreateCredentialCommand(Guid ApplicationId) : IRequest<CreateCredentialCommandResult>;

public record CreateCredentialCommandResult(
    string? Hint,
    Guid KeyId,
    DateTimeOffset? StartDateTime,
    DateTimeOffset? EndDateTime,
    string? Secret);
