using Grpc.Core;
using GrpcStreamingDemo.Proto.CreatePeople;

namespace GrpcStreamingDemo.Web.Services;

public class CreatePeopleService(CreatePeople.CreatePeopleClient createPeopleClient) : CreatePeople.CreatePeopleBase
{
    public override async Task<CreatePeopleResponse> CreatePeople(CreatePeopleRequest request, ServerCallContext context)
        => await createPeopleClient.CreatePeopleAsync(request, cancellationToken: context.CancellationToken);
}