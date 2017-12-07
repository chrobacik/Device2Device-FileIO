using System;
using Device2DeviceFileIO.Classes;
using NUnit.Framework;
using Xamarin.Forms;

namespace Device2DeviceFileIO.Test
{
    [TestFixture()]
    public class QRCodeTest
    {
        [Test()]
        public void QRCodeImageCreationTest()
        {
            var qRCode = new QRCode()
            {
                Url = "http://localhost"
            };

            var imageSource = qRCode.CreateImage(50, 50, 10);

            Assert.Fail();
        }
    }
}
