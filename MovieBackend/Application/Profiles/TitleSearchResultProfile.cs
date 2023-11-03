using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class TitleSearchResultProfile : Profile
{
    public TitleSearchResultProfile()
    {
        CreateMap<TitleSearchResult, TitleSearchResultDTO>();
    }
}