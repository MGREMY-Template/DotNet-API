﻿namespace API.Controllers.Identity;

using Domain.Constants;
using Domain.DataTransferObject;
using Domain.DataTransferObject.Identity;
using Domain.Paging;
using Domain.Queries.Identity.UserToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

[Route("api/Identity/[controller]"), Authorize(Roles = RoleDefinition.ADMIN)]
public class UserTokenController : GenericController
{
    public UserTokenController(
        IMediator mediator) : base(mediator)
    {
    }

    [HttpGet(nameof(GetAll)), Authorize(ClaimDefinition.IDENTITY_USERTOKEN_GETALL)]
    [ProducesResponseType(typeof(Result<UserTokenDto[]>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken = default)
    {
        Result<UserTokenDto[]> result = await this._mediator.Send(new GetUserTokenAllQuery(), cancellationToken);
        return this.StatusCode(result.StatusCode, result);
    }

    [HttpGet(nameof(GetList)), Authorize(ClaimDefinition.IDENTITY_USERTOKEN_GETLIST)]
    [ProducesResponseType(typeof(Result<UserTokenDto[]>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] BasePaging paging,
        CancellationToken cancellationToken = default)
    {
        Result<UserTokenDto[]> result = await this._mediator.Send(new GetUserTokenListQuery(paging), cancellationToken);
        return this.StatusCode(result.StatusCode, result);
    }

    [HttpGet(nameof(GetById)), Authorize(ClaimDefinition.IDENTITY_USERTOKEN_GETBYID)]
    [ProducesResponseType(typeof(Result<UserTokenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<UserTokenDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromQuery] GetUserTokenByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        Result<UserTokenDto> result = await this._mediator.Send(query, cancellationToken);
        return this.StatusCode(result.StatusCode, result);
    }

    [HttpGet(nameof(Count)), Authorize(ClaimDefinition.IDENTITY_USERTOKEN_GETALL)]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count(
        CancellationToken cancellationToken = default)
    {
        var result = await this._mediator.Send(new GetUserTokenCountQuery(), cancellationToken);
        return this.Ok(result);
    }
}
