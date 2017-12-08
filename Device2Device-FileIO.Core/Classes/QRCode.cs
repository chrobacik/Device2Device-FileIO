using System;
using System.IO;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;
using ZXing.Rendering;
using Android.Graphics;

namespace Device2DeviceFileIO.Classes
{
    public class QRCode : BindableBase
    {
        String _Url;
        public String Url
        {
            get { return _Url; }
            set { SetProperty(ref _Url, value); }
        }

        String _FileName;
        public String FileName
        {
            get { return _FileName; }
            set { SetProperty(ref _FileName, value); }
        }

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
                Renderer = new  PixelDataRenderer()

            };

            // Write data to QR code
            var barcode = writer.Write(GetData());
            

            BarCode = ImageSource.FromStream(() => new MemoryStream(barcode.Pixels));

            return _barCode;
        }
    }
}
