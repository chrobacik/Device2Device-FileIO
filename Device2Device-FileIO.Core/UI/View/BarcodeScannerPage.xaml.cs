using System;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using Device2DeviceFileIO.UI.ViewModel;

namespace Device2DeviceFileIO.UI.View
{	
    public partial class BarcodeScannerPage : ContentPage
    {
        public BarcodeScannerVm ViewModel { get; set; }

        public BarcodeScannerPage()
        {
            InitializeComponent();

            Title = "Scan Barcode";

            BindingContext = new BarcodeScannerVm();

            scannerView.OnScanResult += (result) => Device.BeginInvokeOnMainThread(async () => {

                // Stop analysis until we navigate away so we don't keep reading barcodes
                scannerView.IsAnalyzing = false;

                // Show an alert
                await DisplayAlert("Scanned Barcode", result.Text, "OK");

                MessagingCenter.Send<object, string>(this, "QRCodeScanned", result.Text);

                // Navigate away
                await Navigation.PopAsync();
            });

            /*
            mZxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "ZxingScannerView",
                BackgroundColor = Color.Transparent
            };
            */

            /*
            mZxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(async () => {

                // Stop analysis until we navigate away so we don't keep reading barcodes
                mZxing.IsAnalyzing = false;

                // Show an alert
                await DisplayAlert("Scanned Barcode", result.Text, "OK");

                // Navigate away
                await Navigation.PopAsync();
            });
            */

            /*
            mOverlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the barcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = mZxing.HasTorch,
                AutomationId = "ZxingDefaultOverlay",
            };

            defaultOverlay.FlashButtonClicked += (sender, e) => {
                mZxing.IsTorchOn = !mZxing.IsTorchOn;
            };
            */
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            scannerView.IsScanning = true;
            defaultOverlay.ShowFlashButton = scannerView.HasTorch;
        }

        protected override void OnDisappearing()
        {
            scannerView.IsScanning = false;

            base.OnDisappearing();
        }
    }
}
