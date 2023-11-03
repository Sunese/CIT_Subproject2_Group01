using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models;

public class TitleSearchResult
{
    public string TitleID { get; set; }
    public string PrimaryTitle { get; set; }
    public int Rank { get; set; }
}