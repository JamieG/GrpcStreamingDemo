using Google.Protobuf;

namespace GrpcStreamingDemo.Web.Client.Services.FileStorage;

public interface IFileStorageService
{
    Task Begin(string key);
    Task Write(ByteString data);
    Task End();
    Task DownloadFile(string key);
}