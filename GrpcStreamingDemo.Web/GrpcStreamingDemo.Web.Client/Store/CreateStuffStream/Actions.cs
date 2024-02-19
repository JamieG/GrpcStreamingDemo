using Thing = GrpcStreamingDemo.Proto.CreateStuffStream.Thing;

namespace GrpcStreamingDemo.Web.Client.Store.CreateStuffStream;

public record CreateAction(int HowMany);
public record ReceivedAction(Thing Thing);
public record SucceededAction;
public record FailedAction(string Reason);
public record ClearAction;