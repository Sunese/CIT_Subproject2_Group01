using System;
namespace Domain.Models;

public class Principal
{
	public string TitleID { get; set; }
	public Title Title { get; set; }
	public string NameID { get; set; }
	public int Ordering { get; set; }
	public string Category { get; set; }
	public string Job { get; set; }
	// public Name Name { get; set; }
	public List<Character> Characters { get; set; }
}

