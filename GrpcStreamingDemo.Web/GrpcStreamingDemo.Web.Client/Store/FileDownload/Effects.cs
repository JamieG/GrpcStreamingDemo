using Fluxor;
using Grpc.Core;
using GrpcStreamingDemo.Proto.FileDownload;
using GrpcStreamingDemo.Web.Client.Services.FileStorage;

namespace GrpcStreamingDemo.Web.Client.Store.FileDownload;

public class Effects(
    IState<FileDownloadState> fileDownloadState,
    Proto.FileDownload.FileDownload.FileDownloadClient fileDownloadClient,
    IFileStorageService fileStorageService
)
{
    [EffectMethod(typeof(ListFilesAction))]
    public async Task HandleListFilesAction(IDispatcher dispatcher)
    {
        try
        {
            var response = await fileDownloadClient.ListFilesAsync(new());

            dispatcher.Dispatch(new ListFilesSucceededAction(response.Header));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new ListFilesFailedAction());
        }
    }

    [EffectMethod(typeof(DownloadFilesAction))]
    public async Task HandleDownloadFilesAction(IDispatcher dispatcher)
    {
        try
        {
            var call = fileDownloadClient.Download(new()
            {
                Files = { fileDownloadState.Value.Files
                    .Where(file => file.Selected)
                    .Select(file => file.Header.FileName) }
            });

            await foreach (var chunk in call.ResponseStream.ReadAllAsync())
            {
                switch (chunk.ChunkTypeCase)
                {
                    case Chunk.ChunkTypeOneofCase.Header:
                        dispatcher.Dispatch(new ReceivedHeaderChunkAction(chunk.Header));
                        await fileStorageService.Begin(chunk.Header.FileName);
                        break;
                    case Chunk.ChunkTypeOneofCase.Progress:
                        dispatcher.Dispatch(new ReceivedProgressChunkAction(chunk.Progress));
                        if (chunk.Progress.Complete)
                        {
                            await fileStorageService.End();
                            dispatcher.Dispatch(new DownloadCompleteAction(chunk.Progress));
                        }
                        break;
                    case Chunk.ChunkTypeOneofCase.Data:
                        await fileStorageService.Write(chunk.Data.Payload);
                        break;
                }
            }
            
            dispatcher.Dispatch(new DownloadFilesSucceededAction());
        }
        catch (Exception)
        {
            dispatcher.Dispatch(new DownloadFilesFailedAction());
            throw;
        }
    }

    [EffectMethod]
    public async Task HandleDownloadFileAction(DownloadFileAction action, IDispatcher dispatcher)
    {
        await fileStorageService.DownloadFile(action.File.Header.FileName);
    }
}