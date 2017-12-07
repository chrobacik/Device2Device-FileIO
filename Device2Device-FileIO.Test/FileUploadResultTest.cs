using System;
using NUnit.Framework;

namespace Device2DeviceFileIO.Test
{
    [TestFixture()]
    public class FileUploadResultTest
    {
        [Test()]
        public void ParseValidJsonResultTest()
        {
            var expected = new FileIO.FileUploadResult
            {
                Success = true,
                Key = "AbcDeF",
                Link = "https://file.io/AbcDeF"
            };

            var json = "{\"success\":true,\"key\":\"AbcDeF\",\"link\":\"https://file.io/AbcDeF\",\"expiry\":\"14 days\"}";
            var result = FileIO.FileUploadResult.ParseJson(json);

            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void ParseEmptyJsonResultTest()
        {
            var expected = new FileIO.FileUploadResult();

            var json = "{}";
            var result = FileIO.FileUploadResult.ParseJson(json);

            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void ParseInvalidJsonResultTest()
        {
            var expected = new FileIO.FileUploadResult();

            var json = "Merry Christmas!";

            Assert.Catch(() => FileIO.FileUploadResult.ParseJson(json));
        }
    }
}
