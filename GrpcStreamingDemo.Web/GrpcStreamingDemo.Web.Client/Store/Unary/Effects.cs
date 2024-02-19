using Fluxor;
using GrpcStreamingDemo.Proto.Greeter;

namespace GrpcStreamingDemo.Web.Client.Store.Unary;

public class Effects(Greeter.GreeterClient greeterClient)
{
    [EffectMethod]
    public async Task Handle(SayHelloAction action, IDispatcher dispatcher)
    {
        try
        {
            var response = await greeterClient.SayHelloAsync(new ()
            {
                Name = action.Name
            });
            
            dispatcher.Dispatch(new SayHelloSucceededAction(response.Message));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new SayHelloFailedAction(e.Message));
        }
    }
}