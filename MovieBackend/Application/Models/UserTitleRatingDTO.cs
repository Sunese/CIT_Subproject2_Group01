using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Models;

public class UserTitleRatingDTO
{
    public string Username { get; set; }
    public string TitleId { get; set; }
    public int Rating { get; set; }
    public DateTime TimeStamp { get; set; }
    public TitleDTO Title { get; set; }
}