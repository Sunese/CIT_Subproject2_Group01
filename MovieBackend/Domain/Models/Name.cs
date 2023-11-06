using System;
namespace Domain.Models;

public class Name
{
	public string NameId { get; set; }
	public string PrimaryName { get; set; }
	public string BirthYear { get; set; }
	public string DeathYear { get; set;}
	public List<Profession> PrimaryProfessions { get; set; }
	public List<Title> KnownForTitles { get; set; }
	public List<Principal> Principals { get; set; }
}

