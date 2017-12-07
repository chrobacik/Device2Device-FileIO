using System;
using System.Net.Http;

namespace Device2DeviceFileIO.FileIO
{
    public class FileNotFoundException : HttpRequestException
    {
        public FileDownloadErrorResult Result { get; set; }

        public FileNotFoundException(String content)
        {
            Result = FileDownloadErrorResult.ParseJson(content);
        }
    }
}
