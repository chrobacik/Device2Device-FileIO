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
        /*
        [Test]
        public async Task UpDownloadTest()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(path, "TestData\\earth_large.jpg");

            var service = new FileServiceUT();

            FileOperation.UploadFinishedEventArgs args = null;
            service.UploadFinished += (o, a) => args = a;

            service.DownloadProgress += (o, a) => Console.WriteLine($"Download: {a.File.Status.Percentage*100:0}%");
            service.UploadProgress += (o, a) => Console.WriteLine($"Upload: {a.File.Status.Percentage*100:0}%");

            var tf = new TransferFile()
            {
                Name = Path.GetFileName(file),
                Content = File.ReadAllBytes(file)
            };
            var token = new CancellationTokenSource();
            service.Upload(tf);
            await service.UploadFileAsync(token);
            Console.WriteLine("Upload done.");

            Console.WriteLine($"Got dl-url: {args.Code.Url}");

            service.Download(args.Code);
            await service.DownloadFileAsync(token);
            Console.WriteLine("Download done.");


            var url = new QRCode();
        }
        */
    }
}
