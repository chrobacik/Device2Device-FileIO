using System;
using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class BarcodeScannerVm : BindableBase
    {
        
        public BarcodeScannerVm(TransferFile downloadTransferFile, QRCode qRCode)
        {
            DownloadTransferFile = downloadTransferFile;
            QRCode = qRCode;
        }

        public INavigation Navigation { get; set; }
        public ZXing.Result Result { get; set; }

        private TransferFile _downloadTransferFile;
        public TransferFile DownloadTransferFile
        {
            get { return _downloadTransferFile; }
            set { SetProperty(ref _downloadTransferFile, value); }
        }

        private QRCode _qRCode;
        public QRCode QRCode
        {
            get { return _qRCode; }
            set { SetProperty(ref _qRCode, value); }
        }

        private bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set { SetProperty(ref _isAnalyzing, value); }
        }

        private bool _isScanning = true;
        public bool IsScanning
        {
            get { return _isScanning; }
            set { SetProperty(ref _isScanning, value); }
        }

        // lazy instantiation
        private ICommand _tranferFileDownloadCmd;
        public ICommand TranferFileDownloadCommand => _tranferFileDownloadCmd ?? (_tranferFileDownloadCmd = new Command(() => TransferFileDownload()));

        async public void TransferFileDownload()
        {
            await Navigation.PushAsync(new TransferFileDownloadPage(DownloadTransferFile, QRCode));
        }

        // lazy instantiation
        /*
        private ICommand _qRScanResultCommand;
        public ICommand QRScanResultCommand => _qRScanResultCommand ?? (_qRScanResultCommand = new Command(() => QRScanResult()));

        public void QRScanResult()
        {
            IsAnalyzing = false;
            IsScanning = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                Console.WriteLine(Result.Text);

                await Navigation.PopAsync();
            });
        }*/
    }
}
