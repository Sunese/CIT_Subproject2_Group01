using System;
namespace Domain.Models;

public class Episode
{
	public string TitleID { get; set; }
	public Title Title { get; set; }
	public string ParentTitleID { get; set; }
	public Title ParentTitle { get; set; }
	public int EpisodeNumber { get; set; }
	public int SeasonNumber { get; set; }
}