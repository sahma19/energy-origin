using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using EnergyOrigin.Setup.Exceptions;
using Microsoft.Graph;

namespace API.Services;

public interface ICredentialsService
{
    public Task<IEnumerable<ClientCredential>> GetCredentials(Guid applicationId);
    public Task CreateCredential();
    public Task DeleteCredential(string keyId);
}

public class CredentialsService(GraphServiceClient graphServiceClient) : ICredentialsService
{
    public async Task<IEnumerable<ClientCredential>> GetCredentials(Guid applicationId)
    {
        var application = await graphServiceClient.Applications[applicationId.ToString()].Request().GetAsync();
        if (application is null)
        {
            throw new ResourceNotFoundException(applicationId.ToString());
        }

        var clientCredentials = new List<ClientCredential>();
        foreach (var credential in application.PasswordCredentials)
        {
        }
    }

    public Task CreateCredential()
    {
        throw new NotImplementedException();
    }

    public Task DeleteCredential(string keyId)
    {
        throw new NotImplementedException();
    }

    // public async Task CreateClientSecret(Guid applicationId)
    // {
    //     var application = await graphServiceClient.Applications[applicationId.ToString()].Request().GetAsync();
    //     application.PasswordCredentials.FirstOrDefault().
    //     if (application is null)
    //     {
    //         throw new Exception();
    //     }
    //
    //     switch (application.PasswordCredentials.Count())
    //     {
    //        case 0:
    //            break;
    //        case 1:
    //            break;
    //        case > 1:
    //            break;
    //        default:
    //            break;
    //     }
    // }
}
