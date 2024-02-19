using Grpc.Core;
using GrpcStreamingDemo.Proto.CreateStuff;
using GrpcStreamingDemo.Service.Helpers;

namespace GrpcStreamingDemo.Service.Services;

public class CreateStuffService : CreateStuff.CreateStuffBase
{
    public override async Task<CreateStuffResponse> CreateStuff(CreateStuffRequest request, ServerCallContext context)
    {
        var response = new CreateStuffResponse
        {
            Things = { StuffGenerator.Generate(request.HowMany) }
        };

        await Task.Delay(TimeSpan.FromMilliseconds(100 * request.HowMany));
        
        return response;
    }
}