using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models;

public class EpisodeDTO
{
   	public string TitleID { get; set; }
	public TitleDTO Title { get; set; }
	public string ParentTitleID { get; set; }
	public TitleDTO ParentTitle { get; set; }
	public int EpisodeNumber { get; set; }
	public int SeasonNumber { get; set; }
}