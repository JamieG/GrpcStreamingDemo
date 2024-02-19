using GrpcStreamingDemo.Proto.FileDownload;

namespace GrpcStreamingDemo.Web.Client.Store.FileDownload;

public record ListFilesAction;
public record ListFilesSucceededAction(IEnumerable<Header> Files);
public record ListFilesFailedAction;
public record ToggleFileAction(File File);
public record DownloadFilesAction;
public record DownloadFilesFailedAction;
public record DownloadFilesSucceededAction;
public record ReceivedHeaderChunkAction(Header Header);
public record ReceivedProgressChunkAction(Progress Status);
public record DownloadFileAction(File File);
public record ClearDownloadAction(File File);
public record DownloadCompleteAction(Progress Status);