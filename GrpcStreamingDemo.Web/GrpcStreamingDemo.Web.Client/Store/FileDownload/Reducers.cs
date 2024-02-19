using System.Collections.Immutable;
using Fluxor;

namespace GrpcStreamingDemo.Web.Client.Store.FileDownload;

public class Reducers
{
    [ReducerMethod(typeof(ListFilesAction))]
    public static FileDownloadState ReduceListFilesAction(FileDownloadState state)
        => state with { Busy = true, Files = Array.Empty<File>() };

    [ReducerMethod]
    public static FileDownloadState ReduceListFilesSucceededAction(FileDownloadState state,
        ListFilesSucceededAction action)
        => state with
        {
            Busy = false,
            Files = action.Files
                .Select(h => new File(h, false))
                .ToImmutableArray()
        };

    [ReducerMethod(typeof(ListFilesFailedAction))]
    public static FileDownloadState ReduceListFilesFailedAction(FileDownloadState state)
        => state with { Busy = false };

    [ReducerMethod]
    public static FileDownloadState ReduceToggleFileAction(FileDownloadState state, ToggleFileAction action)
        => state with
        {
            Files = state.Files.Select(file =>
                    file.Header.FileName == action.File.Header.FileName
                        ? action.File with { Selected = !action.File.Selected }
                        : file)
                .ToImmutableArray()
        };

    [ReducerMethod(typeof(DownloadFilesAction))]
    public static FileDownloadState ReduceDownloadFilesAction(FileDownloadState state)
        => state with { Busy = true, ActiveDownload = null };

    [ReducerMethod]
    public static FileDownloadState ReduceReceivedHeaderChunkAction(FileDownloadState state,
        ReceivedHeaderChunkAction action)
        => state with
        {
            Busy = true,
            Downloads = state.Downloads.Append(new Download(action.Header.FileName)).ToImmutableArray(),
            ActiveDownload = action.Header.FileName
        };

    [ReducerMethod]
    public static FileDownloadState ReduceReceivedProgressChunkAction(FileDownloadState state,
        ReceivedProgressChunkAction action)
        => state with
        {
            Busy = true,
            Downloads = state.Downloads.Select(
                file => file.File == state.ActiveDownload
                    ? file with
                    {
                        BytesSentTotal = action.Status.BytesSentTotal,
                        BytesRemaining = action.Status.BytesRemaining
                    }
                    : file
            ).ToImmutableArray()
        };

    [ReducerMethod(typeof(DownloadFilesSucceededAction))]
    public static FileDownloadState ReduceDownloadFilesSucceededAction(FileDownloadState state)
        => state with { Busy = false, ActiveDownload = null };

    [ReducerMethod(typeof(DownloadFilesFailedAction))]
    public static FileDownloadState ReduceDownloadFilesFailedAction(FileDownloadState state)
        => state with { Busy = false, ActiveDownload = null };

    [ReducerMethod]
    public static FileDownloadState ReduceClearDownloadAction(FileDownloadState state,
        ClearDownloadAction action)
        => state with
        {
            Downloads = state.Downloads
                .Where(download => download.File != action.File.Header.FileName)
                .ToImmutableList()
        };

    [ReducerMethod]
    public static FileDownloadState ReduceDownloadCompleteAction(FileDownloadState state,
        DownloadCompleteAction action)
        => state with
        {
            Busy = true,
            Files = state.Files.Select(
                file => file.Header.FileName == state.ActiveDownload
                    ? file with
                    {
                        Selected = false
                    }
                    : file
            ),
            Downloads = state.Downloads.Select(
                file => file.File == state.ActiveDownload
                    ? file with
                    {
                        Complete = true
                    }
                    : file
            ).ToImmutableArray()
        };
}