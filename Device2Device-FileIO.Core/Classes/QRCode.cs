using System;
using System.IO;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;
using ZXing.Rendering;

namespace Device2DeviceFileIO.Classes
{
    public class QRCode : BindableBase
    {
        public String Url { get; set; }
        public String FileName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public byte[] Key { get; set; }

        private ImageSource _barCode;
        public ImageSource BarCode
        {
            get { return _barCode; }
            set { SetProperty(ref _barCode, value); }
        }

        public String GetData()
        {
            return Url;
        }

        public ImageSource CreateImage(int width, int height, int margin)
        {
            // Create QR code builder
            var writer = new ZXing.BarcodeWriter<PixelData>
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = margin
                },
                Renderer = new PixelDataRenderer()

            };

            // Write data to QR code
            var barcode = writer.Write(GetData());

            BarCode = ImageSource.FromStream(() => new MemoryStream(barcode.Pixels));
            return _barCode;
        }
    }
}
