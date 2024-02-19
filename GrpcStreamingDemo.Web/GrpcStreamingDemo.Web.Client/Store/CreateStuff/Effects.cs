using Fluxor;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuff;

public class Effects(Proto.CreateStuff.CreateStuff.CreateStuffClient createStuffClient)
{
    [EffectMethod]
    public async Task Handle(CreateAction action, IDispatcher dispatcher)
    {
        try
        {
            var response = await createStuffClient.CreateStuffAsync(new ()
            {
                HowMany = action.HowMany
            });
            
            dispatcher.Dispatch(new SucceededAction(response.Things));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FailedAction(e.Message));
        }
    }
}