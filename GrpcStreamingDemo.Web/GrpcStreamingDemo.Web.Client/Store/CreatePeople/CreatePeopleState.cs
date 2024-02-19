using Fluxor;
using GrpcStreamingDemo.Proto.CreatePeople;

namespace GrpcStreamingDemo.Web.Client.Store.CreatePeople;

[FeatureState]
public record CreatePeopleState(bool Busy, IEnumerable<Person> People, string? FailureReason)
{
    public CreatePeopleState() : this(Busy: false, People: Array.Empty<Person>(), FailureReason: null)
    {
    }
}