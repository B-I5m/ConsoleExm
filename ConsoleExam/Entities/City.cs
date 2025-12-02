namespace ConsoleExam.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Population { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public List<Person> People { get; set; } = new();
}
