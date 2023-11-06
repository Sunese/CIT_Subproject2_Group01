using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using AutoMapper;
using Domain.Models;
using Application.Models;

namespace Application.Profiles;

public class PopularActorsProfile : Profile
{
    public PopularActorsProfile()
    {
        CreateMap<PopularActorsResult, PopularActorsDTO>();
    }
}