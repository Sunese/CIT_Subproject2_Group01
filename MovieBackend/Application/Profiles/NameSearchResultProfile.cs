using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Application.Models;

namespace Application.Profiles;

public class NameSearchResultProfile : Profile
{
    public NameSearchResultProfile()
    {
        CreateMap<Name, NameSearchResultDTO>();
    }
}