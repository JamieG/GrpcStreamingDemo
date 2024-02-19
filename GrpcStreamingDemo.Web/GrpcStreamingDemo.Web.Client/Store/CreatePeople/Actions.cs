using GrpcStreamingDemo.Proto.CreatePeople;

namespace GrpcStreamingDemo.Web.Client.Store.CreatePeople;

public record CreateAction(int HowMany);
public record SucceededAction(IEnumerable<Person> People);
public record FailedAction(string Reason);