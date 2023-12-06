using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface ISearchService
{
    (IList<TitleSearchResultDTO>, int) TitleSearch(string username, string query, TitleType? titleType, int page, int pageSize);
    (IList<NameSearchResultDTO>, int) NameSearch(string username, string query, int page, int pageSize);
    (IList<NameSearchResultDTO>, int) FindActors(string username, string query, int page, int pageSize);
    (IList<NameSearchResultDTO>, int) FindWriters(string username, string query, int page, int pageSize);
    (IList<CoPlayersDTO>, int) FindCoPlayers(string username, string query, int page, int pageSize);
    (IList<UserSearchHistoryDTO>, int) GetUserSearchHistory(string username, int page, int pageSize);
}