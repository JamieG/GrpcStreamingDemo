using Fluxor;
using GrpcStreamingDemo.Proto.CreatePeople;
using GrpcStreamingDemo.Web.Client.Store.CreatePeople;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GrpcStreamingDemo.Web.Client.Pages;

public partial class CreatePeople
{
    [Inject] public IState<CreatePeopleState> CreatePeopleState { get; init; } = null!;
    [Inject] public IDispatcher Dispatcher { get; init; } = null!;
    private bool IsBusy => CreatePeopleState.Value.Busy;
    private bool HasFailed => CreatePeopleState.Value.FailureReason is not null;
    private IEnumerable<Person> Reply => CreatePeopleState.Value.People;
    private void Create(MouseEventArgs _) => Dispatcher.Dispatch(new CreateAction(10));
}