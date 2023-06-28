﻿namespace Application.Handlers.Identity.Role;

using AutoMapper;
using Domain.DataTransferObject;
using Domain.Interface;
using Domain.Queries.Identity.Role;
using Domain.Resources.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Domain.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Domain.DataTransferObject.Identity;
using Domain.Interface.Helper;

public class GetListRoleQueryHandler : IRequestHandler<GetRoleListQuery, Result<RoleDto[]>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IStringLocalizer _globalStringLocalizer;

    public GetListRoleQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        ILogger<GetListRoleQueryHandler> logger,
        IStringLocalizerHelper stringLocalizerHelper)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = logger;
        this._globalStringLocalizer = stringLocalizerHelper.GetStringLocalizer(typeof(GlobalConstants));
    }

    public async Task<Result<RoleDto[]>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        return Result.Create(await this._context.Roles.AsQueryable().ApplyPaging(request.Paging).ToArrayAsync(cancellationToken), 200, 500, this._globalStringLocalizer.GetString(GlobalConstants.InternalServerError))
            .Map(this._mapper.Map<RoleDto[]>);
    }
}
