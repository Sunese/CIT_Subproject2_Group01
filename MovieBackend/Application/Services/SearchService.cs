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

    public IList<NameDTO> NameSearch(string username, string query)
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
            .Where(n => n.PrimaryName.Contains(query))
            .ToList();

        _imdbContext.SaveChanges();
        return _mapper.Map<IList<NameDTO>>(names);
    }

    public IList<TitleDTO> TitleSearch(string username, string query)
    {
        var search = new Search()
        {
            Username = username,
            Query = query,
            Timestamp = DateTime.Now
        };

        _imdbContext.Searches.Add(search);

        var words = query.Split(' ');
        var sqlQuery = new StringBuilder();
        sqlQuery.Append("SELECT best_match_query(");
        foreach (var word in words)
        {
            sqlQuery.Append($"'{word}',");   
        }
        sqlQuery.Remove(sqlQuery.Length - 1, 1);
        sqlQuery.Append(");");

        var titles = _imdbContext.Titles
            .FromSqlRaw(sqlQuery.ToString())
            .ToList();

        _imdbContext.SaveChanges();
        return _mapper.Map<IList<TitleDTO>>(titles);
    }
}