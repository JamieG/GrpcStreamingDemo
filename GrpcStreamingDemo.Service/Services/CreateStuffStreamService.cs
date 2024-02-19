using Grpc.Core;
using GrpcStreamingDemo.Proto.CreateStuffStream;
using GrpcStreamingDemo.Service.Helpers;

namespace GrpcStreamingDemo.Service.Services;

public class CreateStuffStreamService : CreateStuffStream.CreateStuffStreamBase
{
    public override async Task CreateStuff(CreateStuffStreamRequest request, 
        IServerStreamWriter<Thing> responseStream,
        ServerCallContext context)
    {
        foreach (var thing in StuffStreamGenerator.Generate(request.HowMany))
        {
            await responseStream.WriteAsync(thing);
            await Task.Delay(TimeSpan.FromMilliseconds(100));
        }
    }
}