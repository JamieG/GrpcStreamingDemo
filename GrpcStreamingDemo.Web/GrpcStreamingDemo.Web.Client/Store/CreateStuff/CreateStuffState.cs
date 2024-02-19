using Fluxor;
using GrpcStreamingDemo.Proto.CreateStuff;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuff;

[FeatureState]
public record CreateStuffState(bool Busy, IEnumerable<CreateStuffResponse.Types.Thing> Stuff, string? FailureReason)
{
    public CreateStuffState() : this(Busy: false, Stuff: Array.Empty<CreateStuffResponse.Types.Thing>(), FailureReason: null)
    {
    }
}