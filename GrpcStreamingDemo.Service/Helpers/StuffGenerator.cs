using Bogus;
using GrpcStreamingDemo.Proto.CreateStuff;

namespace GrpcStreamingDemo.Service.Helpers;

public static class StuffGenerator
{
    public static IEnumerable<CreateStuffResponse.Types.Thing> Generate(int howMany)
    {
        var fooFaker = new Faker<Foo>()
            .RuleFor(x => x.Text, f => f.Hacker.Noun());
        
        var barFaker = new Faker<Bar>()
            .RuleFor(x => x.Value, f => f.Random.Int());
        
        var faker = new Faker<CreateStuffResponse.Types.Thing>()
            .CustomInstantiator(f =>
                f.Random.Int() % 2 == 0
                    ? new CreateStuffResponse.Types.Thing { Foo = fooFaker.Generate() }
                    : new CreateStuffResponse.Types.Thing { Bar = barFaker.Generate() });
        
        return Enumerable.Range(0, howMany)
            .Select(_ => faker.Generate());
    }
}