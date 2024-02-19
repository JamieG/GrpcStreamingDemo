using Fluxor;
using GrpcStreamingDemo.Proto.CreateStuff;
using GrpcStreamingDemo.Web.Client.Store.CreateStuff;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GrpcStreamingDemo.Web.Client.Pages;

public partial class CreateStuff
{
    [Inject] public IState<CreateStuffState> CreateStuffState { get; init; } = null!;
    [Inject] public IDispatcher Dispatcher { get; init; } = null!;
    private bool IsBusy => CreateStuffState.Value.Busy;
    private bool HasFailed => CreateStuffState.Value.FailureReason is not null;

    private IEnumerable<Foo> Foos => CreateStuffState.Value.Stuff
        .Where(x => x.ThingTypeCase == CreateStuffResponse.Types.Thing.ThingTypeOneofCase.Foo)
        .Select(x => x.Foo);

    private IEnumerable<Bar> Bars => CreateStuffState.Value.Stuff
        .Where(x => x.ThingTypeCase == CreateStuffResponse.Types.Thing.ThingTypeOneofCase.Bar)
        .Select(x => x.Bar);

    private void Create(MouseEventArgs _) => Dispatcher.Dispatch(new CreateAction(20));
}