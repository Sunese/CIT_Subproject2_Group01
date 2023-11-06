using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Models;

public class PrincipalDTO
{
    // public string TitleID { get; set; }
    public PrincipalTitleDTO Title { get; set; }
	// public string NameID { get; set; }
	// public int Ordering { get; set; }
	public string Category { get; set; }
	public string Job { get; set; }
	public List<CharacterDTO> Characters { get; set; }
}