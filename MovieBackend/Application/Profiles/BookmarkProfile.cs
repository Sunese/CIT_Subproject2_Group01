using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class BookmarkProfile : Profile
{
    public BookmarkProfile()
    {
        CreateMap<TitleBookmark, TitleBookmarkDTO>();
        CreateMap<NameBookmark, NameBookmarkDTO>();
    }
}