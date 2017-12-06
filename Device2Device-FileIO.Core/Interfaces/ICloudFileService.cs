using System;
using System.Threading.Tasks;
using System.Threading;
using Device2DeviceFileIO.Classes;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Abstraction of cloud based file storage provider
    /// </summary>
    public interface ICloudFileService
    {
        void Upload(TransferFile file);
        void Upload(TransferFile file, DateTime expiration);
        TransferFile Download(QRCode qRCode);
        TransferFile Download(QRCode qRCode, TransferFile file);

        void CancelUpload();
        void CancelDownload();

        Task UploadFileAsync(CancellationTokenSource cancellation);
        Task DownloadFileAsync(CancellationTokenSource cancellation);

        event EventHandler<FileOperation.UploadProgressEventArgs> UploadProgress;
        event EventHandler<FileOperation.UploadFinishedEventArgs> UploadFinished;
        event EventHandler<FileOperation.DownloadProgressEventArgs> DownloadProgress;
        event EventHandler<FileOperation.DownloadFinsihedEventArgs> DownloadFinished;
    }
}
