using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Application.Models;
using AutoMapper;

namespace API.Profiles;

public class CreateBookmarkProfile : Profile
{
    public CreateBookmarkProfile()
    {
        CreateMap<CreateTitleBookmarkModel, TitleBookmarkDTO>()
            .ForMember(dest => dest.TitleID, opt => opt.MapFrom(src => src.TitleID))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.Now));
        CreateMap<CreateNameBookmarkModel, NameBookmarkDTO>()
            .ForMember(dest => dest.NameID, opt => opt.MapFrom(src => src.NameID))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.Now));
    }
}