using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Services;
using MediatR;

namespace API.Authorization._Features_;

public class GetCredentialsQueryHandler(ICredentialService credentialService)
    : IRequestHandler<GetCredentialsQuery, IEnumerable<GetCredentialsQueryResult>>
{
    public async Task<IEnumerable<GetCredentialsQueryResult>> Handle(GetCredentialsQuery request,
        CancellationToken cancellationToken)
    {
        var credentials = await credentialService.GetCredentials(request.ApplicationId, cancellationToken);

        return credentials.Select(credential => new GetCredentialsQueryResult(credential.Hint, credential.KeyId,
            credential.StartDateTime, credential.EndDateTime)).ToList();
    }
}

public record GetCredentialsQuery(Guid ApplicationId) : IRequest<IEnumerable<GetCredentialsQueryResult>>;

public record GetCredentialsQueryResult(
    string? Hint,
    Guid KeyId,
    DateTimeOffset? StartDateTime,
    DateTimeOffset? EndDateTime);
