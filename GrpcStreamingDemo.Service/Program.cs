using GrpcStreamingDemo.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CreatePeopleService>();
app.MapGrpcService<CreateStuffService>();
app.MapGrpcService<CreateStuffStreamService>();
app.MapGrpcService<FileDownloadService>();

app.Run();