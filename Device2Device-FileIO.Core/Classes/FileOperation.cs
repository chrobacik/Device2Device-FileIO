using System;

namespace Device2DeviceFileIO.Classes
{
    public class FileOperation
    {
        public const string UPLOAD = "FileOperationUpload";
        public const string DOWNLOAD = "FileOperationDownload";

        public class DownloadMessage
        {
        }

        public class UploadMessage
        {
        }

        public class DownloadFinsihedEventArgs : EventArgs
        {
            public TransferFile File { get; set; }
            public QRCode Code { get; set; }
        }

        public class DownloadProgressEventArgs : EventArgs
        {
            public TransferFile File { get; set; }
        }

        public class UploadFinishedEventArgs : EventArgs
        {
            public TransferFile File { get; set; }
            public QRCode Code { get; set; }
        }

        public class UploadProgressEventArgs : EventArgs
        {
            public TransferFile File { get; set; }
        }
    }
}
