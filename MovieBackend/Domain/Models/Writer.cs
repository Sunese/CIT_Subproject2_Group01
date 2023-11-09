using System;
namespace Domain.Models;

public class Writer
{
	public string TitleID { get; set; }
	public Title Title { get; set; }
	public string NameId { get; set; }
	public Name Name { get; set; }
}

