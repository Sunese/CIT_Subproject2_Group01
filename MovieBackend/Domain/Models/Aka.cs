using System;
namespace Domain.Models;

public class Aka
{
    public string TitleId { get; set; }
    public int Ordering { get; set; }
    public Title Title { get; set; }
    public string TitleName { get; set; }
    public string Region { get; set; }
    public string Attribtues { get; set; }
    public string Language { get; set; }
    public bool IsOriginalTitle { get; set; }
    public List<Type> Types { get; set; }
}