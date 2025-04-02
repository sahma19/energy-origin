using System.Threading;
using System.Threading.Tasks;
using API.Services;
using MediatR;

namespace API.Authorization._Features_;

public class CreateClientSecretCommandHandler(ICredentialsService credentialsService)
    : IRequestHandler<CreateClientSecretCommand>
{
    public Task Handle(CreateClientSecretCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}

public record CreateClientSecretCommand : IRequest
{
}
