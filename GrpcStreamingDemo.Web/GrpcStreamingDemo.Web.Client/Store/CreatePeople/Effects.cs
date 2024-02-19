using Fluxor;

namespace GrpcStreamingDemo.Web.Client.Store.CreatePeople;

public class Effects(Proto.CreatePeople.CreatePeople.CreatePeopleClient createPeopleClient)
{
    [EffectMethod]
    public async Task Handle(CreateAction action, IDispatcher dispatcher)
    {
        try
        {
            var response = await createPeopleClient.CreatePeopleAsync(new ()
            {
                HowMany = action.HowMany
            });
            
            dispatcher.Dispatch(new SucceededAction(response.People));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FailedAction(e.Message));
        }
    }
}