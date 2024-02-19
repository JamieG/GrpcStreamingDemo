using BlazorDexie.Database;
using BlazorDexie.JsModule;

namespace GrpcStreamingDemo.Web.Client.Services.FileStorage;

public class FileDb : Db
{
    public Store<File, string> Files { get; set; } = new(nameof(File.Key));
    public Store<FileChunk, Guid> Chunks { get; set; } = new(nameof(FileChunk.Key), nameof(FileChunk.FileKey));
    public Store<byte[], Guid> Blobs { get; set; }= new(string.Empty);

    public FileDb(IModuleFactory moduleFactory)
        : base(nameof(FileDb), 1, new DbVersion[] { }, moduleFactory)
    {
    }
}