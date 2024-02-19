using Fluxor;
using Grpc.Core;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuffStream;

public class Effects(Proto.CreateStuffStream.CreateStuffStream.CreateStuffStreamClient createStuffStreamClient)
{
    [EffectMethod]
    public async Task HandleCreateAction(CreateAction action, IDispatcher dispatcher)
    {
        try
        {
            var call = createStuffStreamClient.CreateStuff(new ()
            {
                HowMany = action.HowMany
            });

            await foreach (var thing in call.ResponseStream.ReadAllAsync())
            {
                dispatcher.Dispatch(new ReceivedAction(thing));
            }
            
            dispatcher.Dispatch(new SucceededAction());
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FailedAction(e.Message));
        }
    }
}