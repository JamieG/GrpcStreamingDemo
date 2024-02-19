using System.Collections.Immutable;
using Fluxor;
using GrpcStreamingDemo.Proto.CreateStuff;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuff;

public static class Reducers
{
    [ReducerMethod(typeof(CreateAction))]
    public static CreateStuffState ReduceCreateAction(CreateStuffState _)
        => new(Busy: true, Stuff: Array.Empty<CreateStuffResponse.Types.Thing>(), FailureReason: null);
    
    [ReducerMethod]
    public static CreateStuffState ReduceSucceededAction(CreateStuffState state, SucceededAction action)
        => state with { Busy = false, Stuff = action.Stuff.ToImmutableArray() };
    
    [ReducerMethod]
    public static CreateStuffState ReduceFailedAction(CreateStuffState state, FailedAction action)
        => state with { Busy = false, FailureReason = action.Reason };
}