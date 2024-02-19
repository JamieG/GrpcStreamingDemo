using Fluxor;

namespace GrpcStreamingDemo.Web.Client.Store.Unary;

public static class Reducers
{
    [ReducerMethod(typeof(SayHelloAction))]
    public static UnaryState ReduceSayHelloAction(UnaryState _)
        => new (Busy: true, Reply: null, FailureReason: null);
    
    [ReducerMethod]
    public static UnaryState ReduceSayHelloSucceededAction(UnaryState state, SayHelloSucceededAction action)
        => state with { Busy = false, Reply = action.Reply };
    
    [ReducerMethod]
    public static UnaryState ReduceSayHelloFailedAction(UnaryState state, SayHelloFailedAction action)
        => state with { Busy = false, FailureReason = action.Reason };
}