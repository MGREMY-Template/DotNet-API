﻿namespace Application.Handlers.Identity.UserToken;

using AutoMapper;
using Domain.DataTransferObject;
using Domain.DataTransferObject.Identity.RoleController;
using Domain.DataTransferObject.Identity.UserTokenController;
using Domain.Extensions;
using Domain.Interface;
using Domain.Queries.Identity.UserToken;
using Domain.Resources.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

public class GetListUserTokenQueryHandler : IRequestHandler<GetUserTokenListQuery, Result<UserTokenDto[]>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IStringLocalizer _globalStringLocalizer;

    public GetListUserTokenQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        ILogger<GetListUserTokenQueryHandler> logger,
        IStringLocalizer<Domain.Resources.Application.Global> globalStringLocalizer)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = logger;
        this._globalStringLocalizer = globalStringLocalizer;
    }

    public async Task<Result<UserTokenDto[]>> Handle(GetUserTokenListQuery request, CancellationToken cancellationToken)
    {
        return Result.Create(await this._context.UserTokens.AsQueryable().ApplyPaging(request.Paging).ToArrayAsync(cancellationToken), 200, 500, this._globalStringLocalizer.GetString(GlobalConstants.InternalServerError))
            .Map(this._mapper.Map<UserTokenDto[]>);
    }
}