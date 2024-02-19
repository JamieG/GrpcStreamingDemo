namespace GrpcStreamingDemo.Web.Client.Services.FileStorage;

public class FileChunk
{
    public Guid Key { get; set; }
    public string FileKey { get; set; } = null!;
    public int ChunkId { get; set; }
    
}