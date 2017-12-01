﻿using System;
using System.IO;
using System.Threading.Tasks;
using Device2DeviceFileIO.Classes;
using NUnit.Framework;

namespace Device2DeviceFileIO.Test
{
    [TestFixture()]
    public class FileServiceTest
    {
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
            var service = new FileIOFileService();
            var stream = this.GetSampleFileStream("Device2DeviceFileIO.Test.Resources.MyFile.txt");

            Task<String> result = service.UploadFileAsync(stream);
            result.Wait();

            Assert.Fail();
        }
    }
}