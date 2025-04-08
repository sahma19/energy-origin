using System;

namespace EnergyOrigin.TokenValidation.b2c;

public class AccessDescriptor
{
    private readonly IdentityDescriptor _identity;

    public AccessDescriptor(IdentityDescriptor identity)
    {
        _identity = identity;
    }

    public bool IsAuthorizedToOrganization(Guid organizationId)
    {
        var isInternalClient = _identity.SubjectType == SubjectType.Internal;
        var isOwnOrganization = _identity.OrganizationId != Guid.Empty && _identity.OrganizationId == organizationId;
        var isAuthorizedToOrganization = _identity.AuthorizedOrganizationIds.Contains(organizationId);
        return isInternalClient || isOwnOrganization || isAuthorizedToOrganization;
    }

    // TODO - Write tests
    public bool IsExternalClient()
    {
        return _identity.SubjectType == SubjectType.External;
    }

    public bool IsAuthorizedToOrganizations(List<Guid> organizationIds)
    {
        return organizationIds.TrueForAll(IsAuthorizedToOrganization);
    }
}
