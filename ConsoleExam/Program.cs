using System.Globalization;
using ConsoleExam.Entities;
var countries = new List<Country>
{
    new Country { Id = 1, Name = "UsA" },
    new Country { Id = 2, Name = "Portugalya" },
    new Country { Id = 3, Name = "Tajikistan" },
    new Country { Id = 4, Name = "SouthKorea" }
};

var cities = new List<City>
{
    new City { Id = 1, Name = "VashingTon", Population = 3200000, CountryId = 1 },
    new City { Id = 2, Name = "NewYork", Population = 1200000, CountryId = 1 },
    new City { Id = 3, Name = "Lisabon", Population = 3900000, CountryId = 2 },
    new City { Id = 4, Name = "Dushanbe", Population = 1400000, CountryId = 3 },
    new City { Id = 5, Name = "Xujand", Population = 1600000, CountryId = 3 },
    new City { Id = 6, Name = "Seul", Population = 1337133, CountryId = 4 }
};

var people = new List<Person>
{
    new Person { Id = 1, FullName = "Trump", Age = 79, CityId = 1 },
    new Person { Id = 2, FullName = "Ilon", Age = 59, CityId = 2 },
    new Person { Id = 3, FullName = "Cristina Runaldu", Age = 41, CityId = 3 },
    new Person { Id = 4, FullName = "Abubakr", Age = 15, CityId = 4 },
    new Person { Id = 5, FullName = "Abubandit", Age = 15, CityId = 5 },
    new Person { Id = 6, FullName = "Kim Chen Ын", Age = 69, CityId = 6 }
};

// TASK 1
var PeoplePopulationFilter = people
        .Join(cities
        .Where(c => c.Population > 3000000), p => p.CityId, c => c.Id, (p, c) => new { PersonName = p.FullName, Age = p.Age,City = c.Name, Population = c.Population });
Console.WriteLine("\nTask1\nPeople with population more than 3mil\n");
foreach (var item in PeoplePopulationFilter)
{
    Console.WriteLine($" {item.PersonName}, население: {item.Population}");
}

//TASK2 
var CitiesPopulationAvg = countries
    .Select(country => new
    {
        Country = country.Name,
        Cities = cities.Where(c => c.CountryId == country.Id) 
            .Where(c => c.Population > cities 
            .Where(x => x.CountryId == country.Id) 
            .Average(x => x.Population)) 
            .ToList() });

Console.WriteLine("\nTask2\nCities with more avg than country\n ");
foreach (var item in CitiesPopulationAvg)
{
    Console.WriteLine(item.Country);
}

//TASK3
    var MinPopulationCities = countries 
        .Select(country => new
        { Country = country.Name, MostPopulatedCity = cities 
            .Where(c => c.CountryId == country.Id) 
            .OrderByDescending(c => c.Population) 
            .First() });
    Console.WriteLine($"\nTask3\nCities which have less population than other cities\n");
    foreach (var item in MinPopulationCities)
    {
        if (item.MostPopulatedCity != null)
            Console.WriteLine($"{item.Country} in {item.MostPopulatedCity.Name}, население: {item.MostPopulatedCity.Population}");
    }
//Task4
    var PeoplesWithCountries = from person in people
        join city in cities on person.CityId equals city.Id
        select new 
        {
            PersonName = person.FullName,
            CityName = city.Name
        };
    Console.WriteLine($"\nTask4\nPeoples with their countries\n");
    foreach (var item in PeoplesWithCountries)
    {
        Console.WriteLine($"{item.PersonName} — {item.CityName}");
    }



//Task6
    var oldestPerCity = cities
        .Select(city => new
        { City = city.Name, OldestPerson = people
                .Where(p => p.CityId == city.Id)
                .OrderByDescending(p => p.Age)
                .FirstOrDefault()
        });
    Console.WriteLine($"\nTask6\nСамый старшый в каждом городе\n");
    foreach (var item in oldestPerCity)
    {
        if (item.OldestPerson != null)
            Console.WriteLine($"{item.City} — {item.OldestPerson.FullName}, {item.OldestPerson.Age} лет");
    }

    //TASK7
    var peopleInMostPopulatedCity = countries
        .SelectMany(country =>
        {
            var city = cities
                .Where(c => c.CountryId == country.Id)
                .OrderByDescending(c => c.Population)
                .FirstOrDefault();

            return people.Where(p => city != null && p.CityId == city.Id);
        });
    Console.WriteLine($"\nTask7\nВсе люди живушие в густонаселеном городе в каждрй стране\n");
    foreach (var person in peopleInMostPopulatedCity)
        Console.WriteLine($"{person.FullName} — {person.CityId}");
   
            //Task9
            var youngestByCountry = from person in people
                join city in cities on person.CityId equals city.Id
                join country in countries on city.CountryId
                    equals country.Id group new { person, city } by country.Name into countryGroup select new
                {
                    Country = countryGroup.Key,
                    YoungestPerson = countryGroup.OrderBy(x => x.person.Age).First().person.FullName,
                    Age = countryGroup.OrderBy(x => x.person.Age).First().person.Age,
                    City = countryGroup.OrderBy(x => x.person.Age).First().city.Name
                };
            Console.WriteLine($"\nTask9\nСамый молодой в каждой стране\n");
            foreach (var item in youngestByCountry)
            {
                Console.WriteLine($"{item.Country}: {item.YoungestPerson} ({item.Age} лет), город: {item.City}");
            }
