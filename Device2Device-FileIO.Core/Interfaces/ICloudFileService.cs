using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Device2DeviceFileIO.Classes;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Abstraction of cloud based file storage provider
    /// </summary>
    public interface ICloudFileService
    {
        void Upload(TransferFile file, QRCode qRCode);
        void Download(TransferFile file, QRCode qRCode);
        Task<String> UploadFileAsync(String fileName, Stream content, CancellationToken token);
        Task<byte[]> DownloadFileAsync(String link, CancellationToken token);

        event EventHandler<FileOperation.DownloadFinishedMessage> DownloadFinished;
        event EventHandler<FileOperation.CanceledMessage> OperationCanceled;
        event EventHandler<FileOperation.ProgressMessage> OperationProgress;
        event EventHandler<FileOperation.FailedMessage> OperationFailed;
        event EventHandler<FileOperation.UploadFinishedMessage> UploadFinished;
    }
}
