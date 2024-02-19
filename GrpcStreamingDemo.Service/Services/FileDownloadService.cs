using System.Collections.Immutable;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GrpcStreamingDemo.Proto.FileDownload;
using Grpc.Core;

namespace GrpcStreamingDemo.Service.Services;

public class FileDownloadService : FileDownload.FileDownloadBase
{
    private const int ChunkSize = 32 * 1024;
    private const long ProgressThreshold = 1024 * 1024; 

    public override Task<ListFilesResponse> ListFiles(Empty request, ServerCallContext context)
    {
        var files = Directory.GetFiles("Files")
            .Select(file => new FileInfo(file))
            .Select(file => new Header
            {
                FileName = file.Name,
                Bytes = file.Length
            });

        return Task.FromResult(new ListFilesResponse { Header = { files } });
    }

    public override async Task Download(DownloadRequest request, IServerStreamWriter<Chunk> responseStream,
        ServerCallContext context)
    {
        var fileInfos = request.Files
            .Select(file => new FileInfo(Path.Combine("Files", file)))
            .ToImmutableArray();

        foreach (var fileInfo in fileInfos)
        {
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File does not exist.", fileInfo.Name);

            await using var reader = fileInfo.OpenRead();

            await responseStream.WriteAsync(new Chunk
            {
                Header = new Header
                {
                    FileName = fileInfo.Name,
                    Bytes = fileInfo.Length
                }
            }, context.CancellationToken);

            var totalBytesSent = 0L;
            var bytesSinceLastProgress = 0L; // Track bytes sent since last progress update
            var buffer = new byte[ChunkSize];
            int bytesRead;
            
            while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length, context.CancellationToken)) > 0)
            {
                await responseStream.WriteAsync(new Chunk
                {
                    Data = new Data
                    {
                        Payload = ByteString.CopyFrom(buffer, 0, bytesRead)
                    }
                }, context.CancellationToken);

                totalBytesSent += bytesRead;
                bytesSinceLastProgress += bytesRead;

                if (bytesSinceLastProgress >= ProgressThreshold)
                {
                    await responseStream.WriteAsync(new Chunk
                    {
                        Progress = new Progress
                        {
                            BytesSentTotal = totalBytesSent,
                            BytesRemaining = fileInfo.Length - totalBytesSent,
                        }
                    }, context.CancellationToken);

                    bytesSinceLastProgress = 0;
                }
            }

            await responseStream.WriteAsync(new Chunk
            {
                Progress = new Progress
                {
                    BytesSentTotal = totalBytesSent,
                    Complete = true
                }
            }, context.CancellationToken);
        }
    }
}