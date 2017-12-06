using System;
using Newtonsoft.Json;

namespace Device2DeviceFileIO.FileIO
{
    /// <summary>
    /// File download error result
    /// 
    /// Example: {"success":false,"error":404,"message":"Not Found"}
    /// </summary>
    [JsonObject]
    public class FileDownloadErrorResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error")]
        public int ErrorCode { get; set; }
        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        public static FileDownloadErrorResult ParseJson(string json)
        {
            return JsonConvert.DeserializeObject<FileDownloadErrorResult>(json);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FileDownloadErrorResult ext = (FileDownloadErrorResult)obj;

            return (Success == ext.Success) && (ErrorCode == ext.ErrorCode) && (ErrorMessage == ext.ErrorMessage);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Success, ErrorCode, ErrorMessage).GetHashCode();
        }

        public override string ToString()
        {
            return $"FileDownloadErrorResult: Success={Success}, ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}";
        }
    }
}
