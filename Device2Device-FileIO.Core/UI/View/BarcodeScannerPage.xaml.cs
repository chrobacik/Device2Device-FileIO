using System;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using Device2DeviceFileIO.UI.ViewModel;
using Device2DeviceFileIO.Classes;

namespace Device2DeviceFileIO.UI.View
{	
    public partial class BarcodeScannerPage : ContentPage
    {
        public BarcodeScannerVm ViewModel { get; set; }

        public BarcodeScannerPage(TransferFile downloadTransferFile, QRCode qRCode)
        {
            InitializeComponent();

            Title = "Scanne QR-Code";

            ViewModel = new BarcodeScannerVm(downloadTransferFile, qRCode);
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            // FIXME: In VM verschieben (stürzt aber leider ab im VM, analysieren wieso)
            scannerView.OnScanResult += (result) => Device.BeginInvokeOnMainThread(async () => {

                // Stop analysis until we navigate away so we don't keep reading barcodes
                scannerView.IsAnalyzing = false;

                // FIXME: new QRCode erstellen aus dem Event (result.Text)

                // Show an alert
                // await DisplayAlert("Scanned Barcode", result.Text, "OK");

                // qRCode = new QRCode();
                // MessagingCenter.Send<object, string>(this, "QRCodeScanned", result.Text);

                qRCode = new QRCode();
                qRCode.Url = result.Text;

                // Navigate away
                await Navigation.PushAsync(new TransferFileDownloadPage(downloadTransferFile, qRCode));
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
