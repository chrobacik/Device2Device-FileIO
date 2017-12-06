using System;
using NUnit.Framework;

namespace Device2DeviceFileIO.Test
{
    [TestFixture()]
    public class FileDownloadErrorResultTest
    {
        [Test()]
        public void ParseValidJsonResultTest()
        {
            var expected = new FileIO.FileDownloadErrorResult
            {
                Success = false,
                ErrorCode = 404,
                ErrorMessage = "Not Found"
            };

            var json = "{\"success\":false,\"error\":404,\"message\":\"Not Found\"}";
            var result = FileIO.FileDownloadErrorResult.ParseJson(json);

            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void ParseEmptyJsonResultTest()
        {
            var expected = new FileIO.FileDownloadErrorResult();

            var json = "{}";
            var result = FileIO.FileDownloadErrorResult.ParseJson(json);

            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void ParseInvalidJsonResultTest()
        {
            var expected = new FileIO.FileDownloadErrorResult();

            var json = "Merry Christmas!";

            Assert.Catch(() => FileIO.FileDownloadErrorResult.ParseJson(json));
        }
    }
}
