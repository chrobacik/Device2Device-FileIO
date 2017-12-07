using System;
using System.IO;
using Xamarin.Forms;

namespace Device2DeviceFileIO.Classes
{
    public class QRCode
    {
        public String Url { get; set; }
        public String FileName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public byte[] Key { get; set; }

        public String GetData()
        {
            return Url;
        }

        public ImageSource CreateImage(int width, int height, int margin)
        {
            // Create QR code builder
            var writer = new ZXing.BarcodeWriter<byte[]>
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = margin
                }
            };

            // Write data to QR code
            var barcode = writer.Write(GetData());

            return ImageSource.FromStream(() => new MemoryStream(barcode));
        }
    }
}
