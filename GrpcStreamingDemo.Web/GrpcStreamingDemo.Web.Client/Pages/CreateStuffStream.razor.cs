using Fluxor;
using GrpcStreamingDemo.Proto.CreateStuffStream;
using GrpcStreamingDemo.Web.Client.Store.CreateStuffStream;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GrpcStreamingDemo.Web.Client.Pages;

public partial class CreateStuffStream
{
    [Inject] public IState<CreateStuffStreamState> CreateStuffStreamState { get; init; } = null!;
    [Inject] public IDispatcher Dispatcher { get; init; } = null!;
    private bool IsBusy => CreateStuffStreamState.Value.Busy;
    private bool HasFailed => CreateStuffStreamState.Value.FailureReason is not null;
    private bool CanClear => Foos.Any() || Bars.Any();

    private IEnumerable<Foo> Foos => CreateStuffStreamState.Value.Stuff
        .Where(x => x.ThingTypeCase == Thing.ThingTypeOneofCase.Foo)
        .Select(x => x.Foo);

    private IEnumerable<Bar> Bars => CreateStuffStreamState.Value.Stuff
        .Where(x => x.ThingTypeCase == Thing.ThingTypeOneofCase.Bar)
        .Select(x => x.Bar);

    private void Create(MouseEventArgs _) => Dispatcher.Dispatch(new CreateAction(20));
    private void Clear(MouseEventArgs _) => Dispatcher.Dispatch(new ClearAction());
}