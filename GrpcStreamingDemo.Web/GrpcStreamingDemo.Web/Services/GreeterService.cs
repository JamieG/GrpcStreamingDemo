using Grpc.Core;
using GrpcStreamingDemo.Proto.Greeter;

namespace GrpcStreamingDemo.Web.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        await Task.Delay(500);
        
        return new HelloReply
        {
            Message = $"Hello `{request.Name}` from server!"
        };
    }
}