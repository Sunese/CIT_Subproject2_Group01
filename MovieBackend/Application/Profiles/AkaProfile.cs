using Application.Models;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles;

public class AkaProfile : Profile
{
    public AkaProfile()
    {
        CreateMap<Aka, AkaDTO>();
    } 
}