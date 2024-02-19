using Fluxor;
using GrpcStreamingDemo.Proto.FileDownload;

namespace GrpcStreamingDemo.Web.Client.Store.FileDownload;

[FeatureState]
public record FileDownloadState(
    bool Busy,
    IEnumerable<File> Files,
    IEnumerable<Download> Downloads,
    string? ActiveDownload)
{
    public FileDownloadState() : this(Busy: false, Files: Array.Empty<File>(), Array.Empty<Download>(), null)
    {
    }
}

public record File(Header Header, bool Selected);

public record Download(
    string File,
    long BytesSentTotal,
    long BytesRemaining,
    bool Complete)
{
    public Download(string file) : this(file, 0, 0, false)
    {
    }
}