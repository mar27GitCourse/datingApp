using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()//map from / to
                .ForMember(dest=>dest.PhotoUrl, opt=>opt.MapFrom(src=>
                    src.Photos.FirstOrDefault(_ => _.IsMain).Url))
                .ForMember(dest=>dest.Age, opt=>
                opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
                    ;//Mapp custom properties (PhotoUrl doesnt exists on the AppUser class)
            CreateMap<Photo, PhotoDto>();
        }
    }
}