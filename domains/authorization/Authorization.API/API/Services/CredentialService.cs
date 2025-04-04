using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using EnergyOrigin.Setup.Exceptions;
using Microsoft.Graph;
using Microsoft.Graph.Applications.Item.AddPassword;
using Microsoft.Graph.Applications.Item.RemovePassword;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace API.Services;

public interface ICredentialService
{
    public Task<IEnumerable<ClientCredential>> GetCredentials(Guid applicationId, CancellationToken cancellationToken);
    public Task<ClientCredential> CreateCredential(Guid applicationId, CancellationToken cancellationToken);
    public Task DeleteCredential(Guid applicationId, Guid keyId, CancellationToken cancellationToken);
}

// TODO: An organization can have multiple app registrations
// TODO: Only allow to change client secret for the app reg that is tied to the access token or all app regs for the organization?
public class CredentialService(GraphServiceClient graphServiceClient) : ICredentialService
{
    public async Task<IEnumerable<ClientCredential>> GetCredentials(Guid applicationId,
        CancellationToken cancellationToken)
    {
        var application = await GetApplication(applicationId, cancellationToken);

        if (application?.PasswordCredentials is null)
        {
            throw new ResourceNotFoundException(applicationId.ToString());
        }

        var clientCredentials = new List<ClientCredential>();
        foreach (var credential in application.PasswordCredentials)
        {
            if (credential.KeyId is null) continue;
            clientCredentials.Add(ClientCredential.Create(credential.Hint, (Guid)credential.KeyId,
                credential.StartDateTime, credential.EndDateTime));
        }

        return clientCredentials;
    }

    public async Task<ClientCredential> CreateCredential(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await GetApplication(applicationId, cancellationToken);
        if (application is null)
        {
            throw new ResourceNotFoundException(applicationId.ToString());
        }

        var body = new AddPasswordPostRequestBody
        {
            PasswordCredential = new PasswordCredential
            {
                DisplayName = "API Secret",
                StartDateTime = DateTimeOffset.UtcNow,
                EndDateTime = DateTimeOffset.UtcNow.AddYears(1)
            }
        };

        try
        {
            var result = await graphServiceClient.Applications[application.Id].AddPassword
                .PostAsync(body, cancellationToken: cancellationToken);

            if (result?.KeyId is null || result.SecretText is null)
            {
                throw new BusinessException("Could not create credential for application");
            }

            return ClientCredential.Create(result.Hint, (Guid)result.KeyId,
                result.StartDateTime,
                result.EndDateTime, result.SecretText);
        }
        catch (ODataError oDataError)
        {
            throw new BusinessException("Could not create credential for application", oDataError);
        }
    }

    public async Task DeleteCredential(Guid applicationId, Guid keyId, CancellationToken cancellationToken)
    {
        try
        {
            var requestBody = new RemovePasswordPostRequestBody
            {
                KeyId = keyId
            };

            await graphServiceClient.Applications[applicationId.ToString()].RemovePassword
                .PostAsync(requestBody, cancellationToken: cancellationToken);
        }
        catch (ODataError oDataError)
        {
            throw new BusinessException("Could not delete credential for application", oDataError);
        }
    }

    private async Task<Application?> GetApplication(Guid applicationId, CancellationToken cancellationToken)
    {
        return await graphServiceClient
            .ApplicationsWithAppId(applicationId.ToString())
            .GetAsync(cancellationToken: cancellationToken);
    }
}
