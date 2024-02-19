using Grpc.Core;
using GrpcStreamingDemo.Proto.CreateStuffStream;

namespace GrpcStreamingDemo.Web.Services;

public class CreateStuffStreamService(CreateStuffStream.CreateStuffStreamClient createStuffStreamClient) : CreateStuffStream.CreateStuffStreamBase
{
    public override async Task CreateStuff(CreateStuffStreamRequest request, IServerStreamWriter<Thing> responseStream, ServerCallContext context)
    {
        var call = createStuffStreamClient.CreateStuff(request,
            cancellationToken: context.CancellationToken);
        
        await foreach (var thing in call.ResponseStream.ReadAllAsync(context.CancellationToken))
        {
            await responseStream.WriteAsync(thing);
        }
    }
}