using Fluxor;
using GrpcStreamingDemo.Proto.CreateStuffStream;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuffStream;

[FeatureState]
public record CreateStuffStreamState(bool Busy, IEnumerable<Thing> Stuff, string? FailureReason)
{
    public CreateStuffStreamState() : this(Busy: false, Stuff: Array.Empty<Thing>(), FailureReason: null)
    {
    }
}