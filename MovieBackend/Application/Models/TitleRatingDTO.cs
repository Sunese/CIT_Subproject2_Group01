using Domain.Models;

namespace Application.Models;

public class TitleRatingDTO
{
    public string TitleID { get; set; }
    public string PrimaryTitle { get; set; }
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
}