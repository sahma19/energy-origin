using System.Threading;
using System.Threading.Tasks;
using API.Repository;
using EnergyOrigin.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Authorization._Features_.Internal;
public record RemoveWhitelistOrganizationCommand(Tin Tin) : IRequest;

public class RemoveWhitelistOrganizationCommandHandler(IWhitelistedRepository whitelistedRepository)
    : IRequestHandler<RemoveWhitelistOrganizationCommand>
{
    public async Task Handle(RemoveWhitelistOrganizationCommand request, CancellationToken cancellationToken)
    {
        var existingEntry = await whitelistedRepository.Query()
            .FirstOrDefaultAsync(w => w.Tin == request.Tin, cancellationToken);

        if (existingEntry != null)
        {
            whitelistedRepository.Remove(existingEntry);
        }
    }
}
