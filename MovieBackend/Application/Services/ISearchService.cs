using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;

namespace Application.Services;

public interface ISearchService
{
    IList<SearchResultDTO> Search(string username, string query);
    IList<TitleSearchResultDTO> TitleSearch(string username, string query);
    IList<NameSearchResultDTO> NameSearch(string username, string query);
}