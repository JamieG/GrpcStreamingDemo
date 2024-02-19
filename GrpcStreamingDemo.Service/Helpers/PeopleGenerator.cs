using Bogus;

using Person = GrpcStreamingDemo.Proto.CreatePeople.Person;

namespace GrpcStreamingDemo.Service.Helpers;

public static class PeopleGenerator
{
    public static IEnumerable<Person> Generate(int howMany)
    {
        var faker = new Faker<Person>()
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName());

        return Enumerable.Range(0, howMany)
            .Select(_ => faker.Generate());
    }
}