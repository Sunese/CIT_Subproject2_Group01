using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models;

public class KnownForTitlesDTO
{
    public string TitleID { get; set; }
    public string PrimaryTitle { get; set; }
    // public string OriginalTitle { get; set; }
    public string TitleType { get; set; }
    // public bool IsAdult { get; set; }
    // public DateOnly? Released { get; set; }
    public int? RuntimeMinutes { get; set; }
    public string? Poster { get; set; }
    // public string? Plot { get; set; }
    // public int? StartYear { get; set; }
    // public int? EndYear { get; set;}
    public List<GenreDTO> Genres { get; set; }
    // public TitleRating TitleRating { get; set; }
}