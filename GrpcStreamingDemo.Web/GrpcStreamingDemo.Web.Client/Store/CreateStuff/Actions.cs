using Thing = GrpcStreamingDemo.Proto.CreateStuff.CreateStuffResponse.Types.Thing;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuff;

public record CreateAction(int HowMany);
public record SucceededAction(IEnumerable<Thing> Stuff);
public record FailedAction(string Reason);