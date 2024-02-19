using System.Collections.Immutable;
using Fluxor;
using GrpcStreamingDemo.Proto.CreateStuffStream;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuffStream;

public static class Reducers
{
    [ReducerMethod(typeof(CreateAction))]
    public static CreateStuffStreamState ReduceCreateAction(CreateStuffStreamState state)
        => state with {Busy = true, FailureReason = null};
    
    [ReducerMethod]
    public static CreateStuffStreamState ReduceReceivedAction(CreateStuffStreamState state, ReceivedAction action)
        => state with { Stuff = state.Stuff.Append(action.Thing).ToImmutableArray()};
    
    [ReducerMethod(typeof(SucceededAction))]
    public static CreateStuffStreamState ReduceSucceededAction(CreateStuffStreamState state)
        => state with { Busy = false };
    
    [ReducerMethod]
    public static CreateStuffStreamState ReduceFailedAction(CreateStuffStreamState state, FailedAction action)
        => state with { Busy = false, FailureReason = action.Reason };
    
    [ReducerMethod(typeof(ClearAction))]
    public static CreateStuffStreamState ReduceClearAction(CreateStuffStreamState state)
        => state with { Stuff = Array.Empty<Thing>()};
}