using System;
using Newtonsoft.Json;

namespace Device2DeviceFileIO.FileIO
{
    /// <summary>
    /// File upload result
    /// 
    /// Example: {\"success\":true,\"key\":\"TwqLtU\",\"link\":\"https://file.io/TwqLtU\",\"expiry\":\"14 days\"}
    /// </summary>
    [JsonObject]
    public class FileUploadResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

        public static FileUploadResult ParseJson(string json)
        {
            return JsonConvert.DeserializeObject<FileUploadResult>(json);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FileUploadResult ext = (FileUploadResult)obj;

            return (Success == ext.Success) && (Key == ext.Key) && (Link == ext.Link);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Success, Key, Link).GetHashCode();
        }

        public override string ToString()
        {
            return $"FileUploadResult: Success={Success}, Key={Key}, Link={Link}";
        }
    }
}
