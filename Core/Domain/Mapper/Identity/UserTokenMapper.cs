﻿namespace Domain.Mapper.Identity;

using AutoMapper;
using Domain.DataTransferObject.Identity;
using Domain.Entities.Identity;

public class UserTokenMapper : Profile
{
    public UserTokenMapper()
    {
        _ = this.CreateMap<UserToken, UserTokenDto>()
            .ForMember(x =>
                x.UserId,
                opt =>
                {
                    opt.MapFrom(src => src.UserId);
                })
            .ForMember(x =>
                x.LoginProvider,
                opt =>
                {
                    opt.MapFrom(src => src.LoginProvider);
                })
            .ForMember(x =>
                x.Value,
                opt =>
                {
                    opt.MapFrom(src => src.Value);
                })
            .ReverseMap();
    }
}
