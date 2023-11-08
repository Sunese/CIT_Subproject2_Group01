using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Models;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class SearchService : ISearchService
{
    private readonly ImdbContext _imdbContext;
    private readonly IMapper _mapper;
    public SearchService(
        ImdbContext imdbContext, 
        IMapper mapper)
    {
        _imdbContext = imdbContext;
        _mapper = mapper;
    }

    public (IList<NameSearchResultDTO>, int) NameSearch(string username, string query, int page, int pageSize)
    {
        // TODO: consider the frontend search functionality might "spam"
        // with search requests, hence a lot of search history records
        // will be created. Consider only storing a search history
        // when the user on the frontend actually clicked something
        var search = new Search()
        {
            Username = username,
            Query = query,
            Timestamp = DateTime.Now
        };
        _imdbContext.Searches.Add(search);

        var names = _imdbContext.Names
            .Where(n => n.PrimaryName.ToLower().Contains(query.ToLower()));
        var paged = names
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        _imdbContext.SaveChanges();
        return (_mapper.Map<IList<NameSearchResultDTO>>(paged), names.Count());
    }

    public (IList<TitleSearchResultDTO>, int) TitleSearch(string username, string query, int page, int pageSize)
    {
        var search = new Search()
        {
            Username = username,
            Query = query,
            Timestamp = DateTime.Now
        };

        _imdbContext.Searches.Add(search);

        // We build the query manually here, because we are not aware
        // of a way for EF to call a Postgres function with a variadic
        // parameter.
        // TODO: this will cause issues if query contains single quotes
        // (and possibly other characters that we have not tested for yet)
        var words = query.Split(' ');
        var sqlQuery = new StringBuilder();
        sqlQuery.Append("SELECT * FROM best_match_query(");
        foreach (var word in words)
        {
            sqlQuery.Append($"'{word}',");
        }
        sqlQuery.Remove(sqlQuery.Length - 1, 1);
        sqlQuery.Append(");");

        var titles = _imdbContext.TitleSearchResults
            .FromSqlRaw(sqlQuery.ToString());
        var paged = titles
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        _imdbContext.SaveChanges();
        return (_mapper.Map<IList<TitleSearchResultDTO>>(paged), titles.Count());
    }

    public (IList<NameSearchResultDTO>, int) FindActors(string username, string query, int page, int pageSize)
    {
        var actors = _imdbContext.NameSearchResults
            .FromSqlInterpolated($"SELECT * FROM find_actors({query}, {username})");
        var paged = actors
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<NameSearchResultDTO>>(paged), actors.Count());
    }

    public (IList<NameSearchResultDTO>, int) FindWriters(string username, string query, int page, int pageSize)
    {
        var writers = _imdbContext.NameSearchResults
            .FromSqlInterpolated($"SELECT * FROM find_writers({query}, {username})");
        var paged = writers
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<NameSearchResultDTO>>(paged), writers.Count());
    }

    public (IList<CoPlayersDTO>, int) FindCoPlayers(string username, string query, int page, int pageSize)
    {
        var coPlayers = _imdbContext.CoPlayers
            .FromSqlInterpolated($"SELECT * FROM co_players({query}, {username})");
        var paged = coPlayers
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();
        return (_mapper.Map<IList<CoPlayersDTO>>(paged), coPlayers.Count());
    }
}