using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileOverviewVm : BindableBase
    {

        public INavigation Navigation { get; set; }

        private TransferFile _uploadTransferFile;
        public TransferFile UploadTransferFile
        {
            get { return _uploadTransferFile; }
            set { SetProperty(ref _uploadTransferFile, value); }
        }

        private TransferFile _downloadTransferFile;
        public TransferFile DownloadTransferFile
        {
            get { return _downloadTransferFile; }
            set { SetProperty(ref _downloadTransferFile, value); }
        }

        // lazy instantiation
        private ICommand _transferFileUploadCmd;
        public ICommand TransferFileUploadCommand => _transferFileUploadCmd ?? (_transferFileUploadCmd = new Command(() => TransferFileUpload()));

        async public void TransferFileUpload()
        {
            await Navigation.PushAsync(new TransferFileUploadPage());
        }

        // lazy instantiation
        private ICommand _readyToReceiveCommand;
        public ICommand ReadyToReceiveCommand => _readyToReceiveCommand ?? (_readyToReceiveCommand = new Command(() => ReadyToReceive()));

        async public void ReadyToReceive()
        {
            await Navigation.PushAsync(new QRCodeScanPage());
        }

        // lazy instantiation
        private ICommand _barcodeScannerCommand;
        public ICommand BarcodeScannerCommand => _barcodeScannerCommand ?? (_barcodeScannerCommand = new Command(() => BarcodeScanner()));

        async public void BarcodeScanner()
        {
            await Navigation.PushAsync(new BarcodeScannerPage());
        }

        // lazy instantiation
        private ICommand _shareCommand;
        public ICommand ShareCommand => _shareCommand ?? (_shareCommand = new Command(() => Share()));

        async public void Share()
        {
            // TODO
        }
    }
}
