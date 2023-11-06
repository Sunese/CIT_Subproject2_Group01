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
    }
}