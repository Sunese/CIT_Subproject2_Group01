using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models;

public class SearchResultDTO
{
    public IList<TitleSearchResultDTO> Titles { get; set; }
    public IList<NameSearchResultDTO> Names { get; set; }
}