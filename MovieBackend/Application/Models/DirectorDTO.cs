using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class DirectorDTO
    {
        public string TitleID { get; set; }
        public TitleDTO Title { get; set; }
        public string NameID { get; set; }
        public NameDTO Name { get; set; }
    }
}