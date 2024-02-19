using System.Diagnostics.CodeAnalysis;
using Fluxor;
using GrpcStreamingDemo.Web.Client.Store.FileDownload;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using File = GrpcStreamingDemo.Web.Client.Store.FileDownload.File;

namespace GrpcStreamingDemo.Web.Client.Pages;

public partial class FileDownload
{
    [Inject] public IState<FileDownloadState> FileDownloadState { get; init; } = null!;
    [Inject] public IDispatcher Dispatcher { get; init; } = null!;
    private bool IsBusy => FileDownloadState.Value.Busy;
    private bool FilesSelected => Files.Any(x => x.Selected);
    private IEnumerable<File> Files => FileDownloadState.Value.Files;
    private IEnumerable<Download> Downloads => FileDownloadState.Value.Downloads;
    private void ListFiles(MouseEventArgs _) => Dispatcher.Dispatch(new ListFilesAction());
    private void ToggleFile(File file) => Dispatcher.Dispatch(new ToggleFileAction(file));
    private void DownloadFiles(MouseEventArgs _) => Dispatcher.Dispatch(new DownloadFilesAction());
    private void DownloadFile(File file) => Dispatcher.Dispatch(new DownloadFileAction(file));
    private void ClearDownload(File file) => Dispatcher.Dispatch(new ClearDownloadAction(file));
    private bool IsDownloading(File file) => IsDownloading(file, out _);
    private bool DownloadComplete(File file) => IsDownloading(file, out var download) && download.Complete;

    private bool IsDownloading(File file, [NotNullWhen(true)] out Download? download)
        => (download = Downloads.FirstOrDefault(x => x.File == file.Header.FileName)) is not null;

    private int DownloadProgress(long bytesSent, long bytesRemaining)
        => (int)Math.Min(100, Math.Max(0, Math.Round(bytesSent / (double)(bytesSent + bytesRemaining) * 100u)));
}