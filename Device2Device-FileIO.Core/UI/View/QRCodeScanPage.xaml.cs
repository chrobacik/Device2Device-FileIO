using System;
using System.Collections.Generic;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.UI.ViewModel;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Device2DeviceFileIO.UI.View
{
    public partial class QRCodeScanPage : ContentPage
    {
        ZXingBarcodeImageView mBarcode;
        public QRCodeScanVm ViewModel { get; set; }

        public QRCodeScanPage(TransferFile uploadTransferFile, QRCode qRCode)
        {
            InitializeComponent();
            Title = "QR-Code zum Scannen";

            //ViewModel = new QRCodeScanVm(uploadTransferFile, qRCode);
            ViewModel = new QRCodeScanVm(App.CurrentUploadFile, App.CurrentUploadQRCode);
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            mBarcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "ZxingBarcodeImageView"
            };

            mBarcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            mBarcode.BarcodeOptions.Width = 300;
            mBarcode.BarcodeOptions.Height = 300;
            mBarcode.BarcodeOptions.Margin = 10;
            mBarcode.BarcodeValue = ViewModel.QRCode.GetData();

            Content = mBarcode;
        }
    }
}
