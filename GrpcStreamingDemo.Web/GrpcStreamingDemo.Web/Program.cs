using GrpcStreamingDemo.Proto.CreatePeople;
using GrpcStreamingDemo.Proto.CreateStuff;
using GrpcStreamingDemo.Proto.CreateStuffStream;
using GrpcStreamingDemo.Proto.FileDownload;
using GrpcStreamingDemo.Web.Client;
using GrpcStreamingDemo.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddGrpc();

AddServiceClient<CreatePeople.CreatePeopleClient>();
AddServiceClient<CreateStuff.CreateStuffClient>();
AddServiceClient<CreateStuffStream.CreateStuffStreamClient>();
AddServiceClient<FileDownload.FileDownloadClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode();

app.UseGrpcWeb();

app.MapGrpcService<GreeterService>().EnableGrpcWeb();
app.MapGrpcService<CreatePeopleService>().EnableGrpcWeb();
app.MapGrpcService<CreateStuffService>().EnableGrpcWeb();
app.MapGrpcService<CreateStuffStreamService>().EnableGrpcWeb();
app.MapGrpcService<FileDownloadService>().EnableGrpcWeb();

app.Run();

void AddServiceClient<T>() where T : class => builder.Services.AddGrpcClient<T>(
    o => o.Address = new Uri("http://demo-service:5001"));
    