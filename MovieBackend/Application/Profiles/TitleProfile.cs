using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class TitleProfile : Profile
{
    public TitleProfile()
    {
        CreateMap<Title, TitleDTO>();
        CreateMap<TitleDTO, Title>();
        CreateMap<Title, PrincipalTitleDTO>();
        CreateMap<Title, KnownForTitlesDTO>();
        CreateMap<Title, TitleRatingDTO>()
            .ForMember(dest => dest.TitleID, opt => opt.MapFrom(src => src.TitleID))
            .ForMember(dest => dest.PrimaryTitle, opt => opt.MapFrom(src => src.PrimaryTitle))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.TitleRating.AverageRating))
            .ForMember(dest => dest.NumVotes, opt => opt.MapFrom(src => src.TitleRating.NumVotes));
    }
}