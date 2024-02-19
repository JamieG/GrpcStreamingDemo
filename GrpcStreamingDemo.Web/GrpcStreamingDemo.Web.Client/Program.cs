using Fluxor;
using Grpc.Net.Client.Web;
using GrpcStreamingDemo.Proto.CreatePeople;
using GrpcStreamingDemo.Proto.CreateStuff;
using GrpcStreamingDemo.Proto.CreateStuffStream;
using GrpcStreamingDemo.Proto.FileDownload;
using GrpcStreamingDemo.Proto.Greeter;
using GrpcStreamingDemo.Web.Client.Services;
using GrpcStreamingDemo.Web.Client.Services.FileStorage;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

AddBffClient<Greeter.GreeterClient>();
AddBffClient<CreatePeople.CreatePeopleClient>();
AddBffClient<CreateStuff.CreateStuffClient>();
AddBffClient<CreateStuffStream.CreateStuffStreamClient>();
AddBffClient<FileDownload.FileDownloadClient>();

builder.Services.AddFluxor(o => o
    .ScanAssemblies(typeof(Program).Assembly)
    .UseReduxDevTools()
);

builder.Services.AddMudServices();
builder.Services.AddFileSystemAccessService();

builder.Services.AddScoped<IFileStorageService, IndexedDbStreamService>();

await builder.Build().RunAsync();

void AddBffClient<T>() where T : class =>
    builder.Services.AddGrpcClient<T>(
            o => o.Address = new Uri(builder.HostEnvironment.BaseAddress))
        .AddHttpMessageHandler(_ => new GrpcWebHandler());
        
        

