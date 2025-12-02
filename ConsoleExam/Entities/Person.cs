namespace ConsoleExam.Entities;

public class Person
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public int Age { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
}