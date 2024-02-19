using Fluxor;
using GrpcStreamingDemo.Web.Client.Store.Unary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GrpcStreamingDemo.Web.Client.Pages;

public partial class Unary
{
    [Inject] public IState<UnaryState> UnaryState { get; init; } = null!;
    [Inject] public IDispatcher Dispatcher { get; init; } = null!;
    private bool IsBusy => UnaryState.Value.Busy;
    private bool HasFailed => UnaryState.Value.FailureReason is not null;
    private bool HasReply => UnaryState.Value.Reply is not null;
    private string? Reply => UnaryState.Value.Reply;
    private void SayHello(MouseEventArgs args) => Dispatcher.Dispatch(new SayHelloAction("Dotnet Oxford"));
}