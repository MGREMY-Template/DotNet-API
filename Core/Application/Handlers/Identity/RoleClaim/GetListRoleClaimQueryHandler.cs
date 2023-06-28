﻿namespace Application.Handlers.Identity.RoleClaim;

using AutoMapper;
using Domain.DataTransferObject;
using Domain.Interface;
using Domain.Queries.Identity.RoleClaim;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Domain.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Resources.Application;
using Domain.DataTransferObject.Identity;
using Domain.Interface.Helper;

public class GetListRoleClaimQueryHandler : IRequestHandler<GetRoleClaimListQuery, Result<RoleClaimDto[]>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IStringLocalizer _globalStringLocalizer;

    public GetListRoleClaimQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        ILogger<GetListRoleClaimQueryHandler> logger,
        IStringLocalizerHelper stringLocalizerHelper)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = logger;
        this._globalStringLocalizer = stringLocalizerHelper.GetStringLocalizer(typeof(GlobalConstants));
    }

    public async Task<Result<RoleClaimDto[]>> Handle(GetRoleClaimListQuery request, CancellationToken cancellationToken)
    {
        return Result.Create(await this._context.RoleClaims.AsQueryable().ApplyPaging(request.Paging).ToArrayAsync(cancellationToken), 200, 500, this._globalStringLocalizer.GetString(GlobalConstants.InternalServerError))
            .Map(this._mapper.Map<RoleClaimDto[]>);
    }
}
