using Grpc.Core;
using GrpcStreamingDemo.Proto.CreatePeople;
using GrpcStreamingDemo.Service.Helpers;

namespace GrpcStreamingDemo.Service.Services;

public class CreatePeopleService : CreatePeople.CreatePeopleBase
{
    public override Task<CreatePeopleResponse> CreatePeople(CreatePeopleRequest request, ServerCallContext context)
    {
        var response = new CreatePeopleResponse
        {
            People = { PeopleGenerator.Generate(request.HowMany) }
        };

        return Task.FromResult(response);
    }
}