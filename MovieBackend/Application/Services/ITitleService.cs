using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface ITitleService
{
    bool TitleExists(string id, out TitleDTO? title);
    bool IsTvSeries(string id);
    IList<WriterDTO> GetWriters(string id);
    IList<DirectorDTO> GetDirectors(string id);
    (IList<TitleDTO> titles, int count) GetTitles(DateOnly startDateTime, DateOnly endDateTime, int count, int page, bool isAdult = false);
    (IList<TitleDTO> titles, int count) GetTvSeries(int page, int pageSize);
    (IList<EpisodeDTO>, int count) GetEpisodes(string id, int page, int pageSize);
    TitleDTO GetTitle(string id, bool isAdult = false);
    (IList<TitleDTO>, int count) GetFeatured(int page, int pageSize);
    (IList<TitleDTO>, int count) GetPopular(int page, int pageSize);
    TitleRatingDTO? GetRating(string id);
    (IList<TitleRatingDTO>, int count) GetRatings(int page, int pageSize, bool orderByHighestRating, int? days);
    (IList<PopularActorsDTO>, int count) GetPopularActors(string titleId, int page, int pageSize);
    (IList<AkaDTO>, int count) GetAkas(string id, int page, int pageSize);
    (IList<SimiliarMoviesResultDTO>, int count) GetSimiliarMovies(string id, int page, int pageSize);
}