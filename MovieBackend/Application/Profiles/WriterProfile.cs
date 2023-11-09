using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class WriterProfile : Profile
{
    public WriterProfile()
    {
        CreateMap<Writer, WriterDTO>();
    }
}