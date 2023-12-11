using System;
namespace Domain.Models;

public class Director
{
	public string TitleID { get; set; }
	public Title Title { get; set; }
	public string NameID { get; set; }
	public Name Name { get; set; }
}

