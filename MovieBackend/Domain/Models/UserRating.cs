﻿using System;
namespace Domain.Models;

public class UserRating
{

    public string Username { get; set; }
    public string TitleId { get; set; }
    public int Rating { get; set; }
    public DateTime TimeStamp { get; set; }
    public Title Title { get; set; }
}

