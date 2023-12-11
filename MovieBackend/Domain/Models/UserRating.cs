using System;
namespace Domain.Models;

public class UserTitleRating
{

    public string Username { get; set; }
    public string TitleID { get; set; }
    public int Rating { get; set; }
    public DateTime TimeStamp { get; set; }
    public Title Title { get; set; }
}

