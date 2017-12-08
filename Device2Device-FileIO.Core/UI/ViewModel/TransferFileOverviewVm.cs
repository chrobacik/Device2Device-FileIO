using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;
using Device2DeviceFileIO.Interfaces;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileOverviewVm : BindableBase
    {

        public TransferFileOverviewVm() {

        }

        

        public void ShareHandler_ShareFileRequestReceived(object sender, System.EventArgs e)
        {
            UploadTransferFile = ((IShareHandler)sender).ReceiveFile();
            TransferFileUpload();
            //await Navigation.PushAsync(new TransferFileUploadPage(UploadTransferFile, QRCode));

        }

        public INavigation Navigation { get; set; }

        private ProgressBar _progressUploadFile;
        public ProgressBar ProgressUploadFile
        {
            get { return _progressUploadFile; }
            set { SetProperty(ref _progressUploadFile, value); }
        }

        private ProgressBar _progressDownloadFile;
        public ProgressBar ProgressDownloadFile
        {
            get { return _progressDownloadFile; }
            set { SetProperty(ref _progressDownloadFile, value); }
        }

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

        private QRCode _qRCode;
        public QRCode QRCode
        {
            get { return _qRCode; }
            set { SetProperty(ref _qRCode, value); }
        }

        // lazy instantiation
        private ICommand _transferFileUploadCmd;
        public ICommand TransferFileUploadCommand => _transferFileUploadCmd ?? (_transferFileUploadCmd = new Command(() => TransferFileUpload()));

        async public void TransferFileUpload()
        {
            await Navigation.PushAsync(new TransferFileUploadPage(UploadTransferFile, QRCode));
        }

        // lazy instantiation
        private ICommand _readyToReceiveCommand;
        public ICommand ReadyToReceiveCommand => _readyToReceiveCommand ?? (_readyToReceiveCommand = new Command(() => ReadyToReceive()));

        async public void ReadyToReceive()
        {
            await Navigation.PushAsync(new QRCodeScanPage(UploadTransferFile, QRCode));
        }

        // lazy instantiation
        private ICommand _barcodeScannerCommand;
        public ICommand BarcodeScannerCommand => _barcodeScannerCommand ?? (_barcodeScannerCommand = new Command(() => BarcodeScanner()));

        async public void BarcodeScanner()
        {
            await Navigation.PushAsync(new BarcodeScannerPage(DownloadTransferFile));
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
