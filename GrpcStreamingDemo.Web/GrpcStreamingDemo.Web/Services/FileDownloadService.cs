using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcStreamingDemo.Proto.FileDownload;

namespace GrpcStreamingDemo.Web.Services;

public class FileDownloadService(FileDownload.FileDownloadClient client) : FileDownload.FileDownloadBase
{
    public override async Task<ListFilesResponse> ListFiles(Empty request, ServerCallContext context)
        => await client.ListFilesAsync(request, cancellationToken: context.CancellationToken);

    public override async Task Download(DownloadRequest request, IServerStreamWriter<Chunk> responseStream, ServerCallContext context)
    {
        var call = client.Download(request, cancellationToken: context.CancellationToken);

        await foreach (var chunk in call.ResponseStream.ReadAllAsync())
        {
            await responseStream.WriteAsync(chunk, context.CancellationToken);
        }
    }
}