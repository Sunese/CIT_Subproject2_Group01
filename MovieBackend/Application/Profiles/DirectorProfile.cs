using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class DirectorProfile : Profile
{
    public DirectorProfile()
    {
        CreateMap<Director, DirectorDTO>();
    }
}