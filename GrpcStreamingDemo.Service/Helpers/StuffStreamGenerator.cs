using Bogus;
using GrpcStreamingDemo.Proto.CreateStuffStream;

namespace GrpcStreamingDemo.Service.Helpers;

public static class StuffStreamGenerator
{
    public static IEnumerable<Thing> Generate(int howMany)
    {
        var fooFaker = new Faker<Foo>()
            .RuleFor(x => x.Text, f => f.Hacker.Noun());
        
        var barFaker = new Faker<Bar>()
            .RuleFor(x => x.Value, f => f.Random.Int());
        
        var faker = new Faker<Thing>()
            .CustomInstantiator(f =>
                f.Random.Int() % 2 == 0
                    ? new Thing { Foo = fooFaker.Generate() }
                    : new Thing { Bar = barFaker.Generate() });
        
        return Enumerable.Range(0, howMany)
            .Select(_ => faker.Generate());
    }
}