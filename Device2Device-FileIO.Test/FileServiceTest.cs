using System;
using System.IO;
using System.Threading.Tasks;
using Device2DeviceFileIO.Classes;
using NUnit.Framework;

namespace Device2DeviceFileIO.Test
{
    [TestFixture()]
    public class FileServiceTest
    {
        public class FileServiceUT : FileIO.FileService
        {
            public String _BuildUploadURL(QRCode qRCode)
            {
                return this.BuildUploadURL(qRCode);
            }
        }
        
        public Stream GetSampleFileStream(string name)
        {
            var assembly = this.GetType().Assembly;

            return assembly.GetManifestResourceStream(name);
        }

        [SetUp()]
        public void SetUp()
        {
        }

        [TearDown()]
        public void TearDown()
        {
        }

        [Test()]
        public void SendFileTest()
        {
            var service = new FileIO.FileService();
            var stream = this.GetSampleFileStream("Device2DeviceFileIO.Test.Resources.MyFile.txt");

            Assert.Fail();
        }

        [Test()]
        public void BuildUploadURLTest()
        {
            var service = new FileServiceUT();
            var expected = "https://file.io/?expires=14";

            Assert.AreEqual(expected, service._BuildUploadURL(new QRCode()));
        }
    }
}
