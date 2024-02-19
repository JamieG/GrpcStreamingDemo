using Grpc.Core;
using GrpcStreamingDemo.Proto.CreateStuff;

namespace GrpcStreamingDemo.Web.Services;

public class CreateStuffService(CreateStuff.CreateStuffClient createStuffClient) : CreateStuff.CreateStuffBase
{
    public override async Task<CreateStuffResponse> CreateStuff(CreateStuffRequest request, ServerCallContext context)
        => await createStuffClient.CreateStuffAsync(request, cancellationToken: context.CancellationToken);
}