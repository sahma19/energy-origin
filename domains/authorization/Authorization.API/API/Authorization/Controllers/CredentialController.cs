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
public class CredentialController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates credentials for an App Registration in Azure B2C.
    /// </summary>
    [HttpPost]
    [Route("api/authorization/credentials/")]
    [ProducesResponseType(typeof(CreateCredentialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateCredential([FromBody] CreateCredentialRequest request)
    {
        var commandResult =
            await mediator.Send(new CreateCredentialCommand(request.ApplicationId));

        var response = new CreateCredentialResponse(commandResult.Hint, commandResult.KeyId,
            commandResult.StartDateTime, commandResult.EndDateTime, commandResult.Secret);

        return Ok(response);
    }

    /// <summary>
    ///
    /// </summary>
    [HttpGet]
    [Route("api/authorization/credentials/")]
    [ProducesResponseType(typeof(IEnumerable<GetCredentialsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetCredentials([FromBody] GetCredentialsRequest request)
    {
        var queryResult = await mediator.Send(new GetCredentialsQuery(request.ApplicationId));

        var response = queryResult.Select(credential => new GetCredentialsResponse(credential.Hint, credential.KeyId,
            credential.StartDateTime, credential.EndDateTime)).ToList();

        return Ok(response);
    }
}
