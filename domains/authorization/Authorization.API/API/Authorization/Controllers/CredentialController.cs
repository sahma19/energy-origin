using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Authorization._Features_;
using Asp.Versioning;
using EnergyOrigin.Setup;
using EnergyOrigin.TokenValidation.b2c;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Authorization.Controllers;

[ApiController]
[ApiVersion(ApiVersions.Version1)]
[Authorize(Policy.FrontendOr3rdParty)]
[Route("api/authorization/clients/{clientId:guid}/credentials")]
public class CredentialController(
    IMediator mediator,
    AccessDescriptor accessDescriptor,
    IdentityDescriptor identityDescriptor) : ControllerBase
{
    /// <summary>
    /// Creates a single credential for a client.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateCredentialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateCredential([FromRoute] Guid clientId)
    {
        if (!accessDescriptor.IsExternalClientAuthorized())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var commandResult =
            await mediator.Send(new CreateCredentialCommand(clientId, identityDescriptor.OrganizationId));

        var response = new CreateCredentialResponse(commandResult.Hint, commandResult.KeyId,
            commandResult.StartDateTime, commandResult.EndDateTime, commandResult.Secret);

        return Ok(response);
    }

    /// <summary>
    /// Gets all credentials for a client.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetCredentialsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetCredentials([FromRoute] Guid clientId)
    {
        if (!accessDescriptor.IsExternalClientAuthorized())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var queryResult = await mediator.Send(new GetCredentialsQuery(clientId, identityDescriptor.OrganizationId));

        var response = queryResult.Select(credential => new GetCredentialsResponse(credential.Hint, credential.KeyId,
            credential.StartDateTime, credential.EndDateTime)).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Deletes a single credential for a client.
    /// </summary>
    [HttpDelete]
    [Route("{keyId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<GetCredentialsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCredential([FromRoute] Guid clientId, [FromRoute] Guid keyId)
    {
        if (!accessDescriptor.IsExternalClientAuthorized())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        await mediator.Send(new DeleteCredentialCommand(clientId, keyId, identityDescriptor.OrganizationId));

        return Ok();
    }
}
