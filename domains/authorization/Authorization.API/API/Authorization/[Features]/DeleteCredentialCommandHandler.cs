using System;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using MediatR;

namespace API.Authorization._Features_;

public class DeleteCredentialCommandHandler(ICredentialService credentialService) : IRequestHandler<DeleteCredentialCommand>
{
    public Task Handle(DeleteCredentialCommand request, CancellationToken cancellationToken)
    {
        return credentialService.DeleteCredential(request.ApplicationId, request.KeyId, cancellationToken);
    }
}

public record DeleteCredentialCommand(Guid ApplicationId, Guid KeyId) : IRequest;
