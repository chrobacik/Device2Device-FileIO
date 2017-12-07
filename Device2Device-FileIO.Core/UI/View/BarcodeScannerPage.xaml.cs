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

            ViewModel = new BarcodeScannerVm();
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            scannerView.OnScanResult += (result) => Device.BeginInvokeOnMainThread(async () => {

                // Stop analysis until we navigate away so we don't keep reading barcodes
                scannerView.IsAnalyzing = false;

                // Show an alert
                await DisplayAlert("Scanned Barcode", result.Text, "OK");

                MessagingCenter.Send<object, string>(this, "QRCodeScanned", result.Text);

                // Navigate away
                await Navigation.PushAsync(new TransferFileDownloadPage());
            });
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
