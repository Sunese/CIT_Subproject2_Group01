using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CreateTitleRatingModel
    {
        public string TitleID { get; set; }
        public int Rating { get; set; }
    }
}