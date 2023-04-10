﻿namespace Application.Handlers.Identity.UserLogin;

using AutoMapper;
using Domain.DataTransferObject;
using Domain.DataTransferObject.Identity.UserLoginController;
using Domain.Interface;
using Domain.Queries.Identity.UserLogin;
using Domain.Resources.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Domain.Extensions;
using System.Threading;
using System.Threading.Tasks;

public class GetAllUserLoginQueryHandler : IRequestHandler<GetUserLoginAllQuery, Result<UserLoginDto[]>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IStringLocalizer _globalStringLocalizer;

    public GetAllUserLoginQueryHandler(
        IAppDbContext context,
        IMapper mapper,
        ILogger<GetAllUserLoginQueryHandler> logger,
        IStringLocalizer<Domain.Resources.Application.Global> globalStringLocalizer)
    {
        this._context = context;
        this._mapper = mapper;
        this._logger = logger;
        this._globalStringLocalizer = globalStringLocalizer;
    }

    public async Task<Result<UserLoginDto[]>> Handle(GetUserLoginAllQuery request, CancellationToken cancellationToken)
    {
        return Result.Create(await this._context.UserLogins.ToArrayAsync(cancellationToken), 200, 400, this._globalStringLocalizer.GetString(GlobalConstants.InternalServerError))
            .Map(this._mapper.Map<UserLoginDto[]>);
    }
}