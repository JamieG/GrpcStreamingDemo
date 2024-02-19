using BlazorDexie.JsModule;
using Google.Protobuf;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.JSInterop;

namespace GrpcStreamingDemo.Web.Client.Services.FileStorage;

public class IndexedDbStreamService : IFileStorageService, IAsyncDisposable
{
    private readonly FileDb _db;
    private readonly IFileSystemAccessService _fileSystem;
    private readonly MemoryStream _buffer = new();
    private readonly ILogger<IndexedDbStreamService> _logger;

    private string? _currentFile;

    public IndexedDbStreamService(IJSRuntime js, IFileSystemAccessService fileSystemAccessService,
        ILogger<IndexedDbStreamService> logger)
    {
        _db = new FileDb(new EsModuleFactory(js));
        _fileSystem = fileSystemAccessService;
        _logger = logger;

        Purge();
    }

    public async Task Begin(string key)
    {
        _buffer.Seek(0, SeekOrigin.Begin);
        _buffer.SetLength(0);

        _currentFile = key;

        await ClearFileIfExists(key);
    }

    private void Purge()
    {
        _db.Files.Clear();
        _db.Chunks.Clear();
        _db.Blobs.Clear();
    }

    private async Task ClearFileIfExists(string key)
    {
        var file = await _db.Files.Get(key);
        if (file is not null)
        {
            var chunks = await _db.Chunks
                .Where(new Dictionary<string, object>() { { nameof(FileChunk.FileKey), key } })
                .ToList();

            foreach (FileChunk chunk in chunks)
            {
                await _db.Blobs.Delete(chunk.Key);
                await _db.Chunks.Delete(chunk.Key);
            }

            await _db.Files.Delete(file.Key);
        }
    }

    public async Task Write(ByteString data)
    {
        if (_currentFile is null)
            throw new InvalidOperationException("No current file");

        await _buffer.WriteAsync(data.Memory);

        if (_buffer.Length > 10 * 1024 * 1024)
        {
            await WriteToStream(_currentFile, _buffer);

            _buffer.Seek(0, SeekOrigin.Begin);
            _buffer.SetLength(0);
        }
    }

    public async Task End()
    {
        if (_currentFile is null)
            throw new InvalidOperationException("No current file");

        if (_buffer.Length > 0)
            await WriteToStream(_currentFile, _buffer);

        _buffer.Seek(0, SeekOrigin.Begin);
        _buffer.SetLength(0);
    }

    private async Task WriteToStream(string key, MemoryStream stream)
    {
        var file = await _db.Files.Get(key) ?? new File { Key = key };

        var chunkId = file.NoChunks;
        var chunkKey = Guid.NewGuid();

        await _db.Blobs.PutBlob(stream.ToArray(), chunkKey);

        await _db.Chunks.Put(new FileChunk
        {
            Key = chunkKey,
            FileKey = key,
            ChunkId = chunkId
        });

        file.NoChunks += 1;

        await _db.Files.Put(file, key);
    }

    public async Task DownloadFile(string key)
    {
        var chunks = await _db.Chunks
            .Where(new Dictionary<string, object> { { nameof(FileChunk.FileKey), key } })
            .ToList();

        try
        {
            var fileHandle = await _fileSystem.ShowSaveFilePickerAsync(
                new SaveFilePickerOptionsStartInWellKnownDirectory()
                {
                    StartIn = WellKnownDirectory.Downloads,
                    SuggestedName = key,
                });

            var writable = await fileHandle.CreateWritableAsync();

            foreach (var chunk in chunks.OrderBy(x => x.ChunkId))
            {
                var blob = await _db.Blobs.GetBlob(chunk.Key);
                await writable.WriteAsync(blob);
            }

            await writable.CloseAsync();
        }
        catch (JSException e) when (e.Message.Contains("The user aborted a request"))
        {
           // Ignore user dialog aborts.
        }
    }

    public async ValueTask DisposeAsync()
    {
        Purge();

        await _db.DisposeAsync();
        await _fileSystem.DisposeAsync();
        await _buffer.DisposeAsync();
    }
}