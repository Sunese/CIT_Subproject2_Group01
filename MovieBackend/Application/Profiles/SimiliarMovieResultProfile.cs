using Application.Models;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Profiles;

public class SimiliarMovieResultProfile : Profile
{
    public SimiliarMovieResultProfile()
    {
        CreateMap<SimiliarMoviesResult, SimiliarMoviesResultDTO>();
    }
}