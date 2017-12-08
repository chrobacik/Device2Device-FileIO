using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Device2DeviceFileIO
{
    public partial class BarcodePage : ContentPage
    {
        ZXingBarcodeImageView mBarcode;

        public BarcodePage()
        {
            InitializeComponent();
            Title = "Show Barcode";
            //BackgroundColor = Color.White;

            mBarcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "ZxingBarcodeImageView",
                BackgroundColor = Color.Transparent
            };

            mBarcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            mBarcode.BarcodeOptions.Width = 300;
            mBarcode.BarcodeOptions.Height = 300;
            mBarcode.BarcodeOptions.Margin = 10;
            mBarcode.BarcodeValue = "ZXing.Net.Mobile";

            Content = mBarcode;
        }
    }
}
