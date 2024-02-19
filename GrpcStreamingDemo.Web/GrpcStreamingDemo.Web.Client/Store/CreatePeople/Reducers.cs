using System.Collections.Immutable;
using Fluxor;
using GrpcStreamingDemo.Proto.CreatePeople;

namespace GrpcStreamingDemo.Web.Client.Store.CreatePeople;

public static class Reducers
{
    [ReducerMethod(typeof(CreateAction))]
    public static CreatePeopleState ReduceCreateAction(CreatePeopleState _)
        => new (Busy: true, People: Array.Empty<Person>(), FailureReason: null);
    
    [ReducerMethod]
    public static CreatePeopleState ReduceSucceededAction(CreatePeopleState state, SucceededAction action)
        => state with { Busy = false, People = action.People.ToImmutableArray() };
    
    [ReducerMethod]
    public static CreatePeopleState ReduceFailedAction(CreatePeopleState state, FailedAction action)
        => state with { Busy = false, FailureReason = action.Reason };
}