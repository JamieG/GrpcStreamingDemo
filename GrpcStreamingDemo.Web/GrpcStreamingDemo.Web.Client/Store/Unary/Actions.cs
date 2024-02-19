namespace GrpcStreamingDemo.Web.Client.Store.Unary;

public record SayHelloAction(string Name);
public record SayHelloSucceededAction(string Reply);
public record SayHelloFailedAction(string Reason);