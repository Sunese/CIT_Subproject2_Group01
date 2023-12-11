using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models;

public class CreateTitleBookmarkModel
{
    public string TitleID { get; set; }
    public string Notes { get; set; }
}