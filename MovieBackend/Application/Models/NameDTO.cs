using Domain.Models;

namespace Application.Models;

public class NameDTO
{
    public string NameId { get; set; }
	public string PrimaryName { get; set; }
	public string BirthYear { get; set; }
	public string DeathYear { get; set;}
	// public List<ProfessionDTO> PrimaryProfessions { get; set; }
	// public List<KnownForTitlesDTO> KnownForTitles { get; set; }
	// public List<PrincipalDTO> Principals { get; set; }
}