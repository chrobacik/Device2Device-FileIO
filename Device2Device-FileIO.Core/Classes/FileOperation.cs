using System;

namespace Device2DeviceFileIO.Classes
{
    public class FileOperation
    {
        public const string DOWNLOAD = "FileOperationDownload";
        public const string CANCELED = "FileOperationCanceled";
        public const string DOWNLOAD_FINISHED = "FileOperationDownloadFinished";
        public const string UPLOAD = "FileOperationUpload";
        public const string PROGRESS = "FileOperationProgress";
        public const string FAILED = "FileOperationFailed";
        public const string UPLOAD_FINISHED = "FileOperationUploadFinished";

        public class CanceledMessage
        {
        }

        public class FailedMessage
        {
            public String Error { get; set; }
        }

        public class DownloadMessage
        {
            public String Link { get; set; }
        }

        public class DownloadFinishedMessage
        {
            public byte[] Content { get; set; }
        }

        public class ProgressMessage
        {
            public float Percentage { get; set; }
        }

        public class UploadMessage
        {
            public String FileName { get; set; }
            public byte[] Content { get; set; }
        }

        public class UploadFinishedMessage
        {
            public String Result { get; set; }
        }
    }
}
