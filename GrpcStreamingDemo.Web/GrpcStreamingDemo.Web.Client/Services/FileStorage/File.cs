namespace GrpcStreamingDemo.Web.Client.Services.FileStorage;

public class File
{
    public string Key { get; set; } = null!;
    public int NoChunks { get; set; }
}