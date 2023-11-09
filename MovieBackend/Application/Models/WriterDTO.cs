using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class WriterDTO
    {
    public string TitleID { get; set; }
	public TitleDTO Title { get; set; }
	public string NameId { get; set; }
	public NameDTO Name { get; set; }
    }
}