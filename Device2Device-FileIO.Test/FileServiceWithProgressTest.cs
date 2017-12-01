using System;
using System.IO;
using System.Threading.Tasks;
using Device2DeviceFileIO.net;
using NUnit.Framework;

namespace Device2DeviceFileIO.Test
{
    public class FileServiceWithProgressTest
    {

        public Stream GetSampleFileStream(string name)
        {
            var assembly = this.GetType().Assembly;

            return assembly.GetManifestResourceStream(name);
        }


        [Test]
        public async Task FileUploadWithProgressFromStreamTest()
        {
            var stream = this.GetSampleFileStream("Device2DeviceFileIO.Test.Resources.MyFile.txt");
            var client = new FileIoClientWithProgress();
            client.HttpSendProgress += (o, a) =>
            {
                // notify about progress...
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.sss}: Uploaded {a.BytesTransferred} of {a.TotalBytes} Bytes ({(a.BytesTransferred * 100 / a.TotalBytes)})");
            };

            var result = await client.UploadFileFromStream(stream, "MyFile.txt");
            Console.WriteLine(result);
        }

        [Test]
        public async Task FileUploadWithProgressFromFileTest()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var file = System.IO.Path.Combine(path, "testdata", "earth_large.jpg");
            var client = new FileIoClientWithProgress();
            client.HttpSendProgress += (o, a) =>
            {
                // notify about progress...
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.sss}: Uploaded {a.BytesTransferred} of {a.TotalBytes} Bytes ({(a.BytesTransferred * 100 / a.TotalBytes)})");
            };

            var result = await client.UploadFile(file);
            Console.WriteLine(result);
        }
        
        
    }
}