using System;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Device2DeviceFileIO
{	
	public partial class BarcodeScanPage : ContentPage
    {
        ZXingScannerView mZxing;
        ZXingDefaultOverlay mOverlay;

        public BarcodeScanPage()
        {
            InitializeComponent();
            Title = "Scan Barcode";

            mZxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "ZxingScannerView",
                BackgroundColor = Color.Transparent
            };

            mZxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(async () => {

                // Stop analysis until we navigate away so we don't keep reading barcodes
                mZxing.IsAnalyzing = false;

                // Show an alert
                await DisplayAlert("Scanned Barcode", result.Text, "OK");

                // Navigate away
                await Navigation.PopAsync();
            });

            mOverlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the barcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = mZxing.HasTorch,
                AutomationId = "ZxingDefaultOverlay",
            };

            mOverlay.FlashButtonClicked += (sender, e) => {
                mZxing.IsTorchOn = !mZxing.IsTorchOn;
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            grid.Children.Add(mZxing);
            grid.Children.Add(mOverlay);

            // The root page of your application
            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            mZxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            mZxing.IsScanning = false;

            base.OnDisappearing();
        }
    }
}
