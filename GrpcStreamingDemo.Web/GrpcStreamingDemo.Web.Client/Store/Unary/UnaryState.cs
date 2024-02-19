using Fluxor;

namespace GrpcStreamingDemo.Web.Client.Store.Unary;

[FeatureState]
public record UnaryState(bool Busy, string? Reply, string? FailureReason)
{
    public UnaryState() : this(Busy: false, Reply: null, FailureReason: null)
    {
    }
}