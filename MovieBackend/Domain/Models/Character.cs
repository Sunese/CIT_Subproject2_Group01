using System;
namespace Domain.Models;

public class Character
{
	public string TitleID { get; set; }
	public int Ordering { get; set; }
	public string CharacterName { get; set; }
	public Principal Principal { get; set; }
}

