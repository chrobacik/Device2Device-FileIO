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
            DownloadTransferFile = App.CurrentDownloadFile;
            QRCode = App.CurrentDownloadQRCode;
        }

        public void ShareHandler_ShareFileRequestReceived(object sender, System.EventArgs e)
        {
            UploadTransferFile = ((IShareHandler)sender).ReceiveFile();
            TransferFileUpload();
            //await Navigation.PushAsync(new TransferFileUploadPage(UploadTransferFile, QRCode));

        }

        public void DownloadHandler(object sender, FileOperation.DownloadFinsihedEventArgs e)
        {
            ProgressDownloadFile.ProgressTo(1.0, 250, Easing.Linear);
            if (e.File != null)
            {
                DownloadTransferFile.Name = e.File.Name;
                DownloadTransferFile.Size = e.File.Size;
                DownloadTransferFile.Type = e.File.Type;
                DownloadTransferFile.Content = e.File.Content;
                DownloadTransferFile.Status.State = e.File.Status.State;
            } else {
                DownloadTransferFile.Status.State = e.File.Status.State;
            }
        }

        public void DownloadProgressHandler(object sender, FileOperation.DownloadProgressEventArgs e)
        {
            ProgressDownloadFile.ProgressTo(e.File.Status.Percentage, 250, Easing.Linear);
        }

        public INavigation Navigation { get; set; }

        private bool _isBtnReadyToReceiveEnabled = true;
        public bool IsBtnReadyToReceiveEnabled
        {
            get { return _isBtnReadyToReceiveEnabled; }
            set { SetProperty(ref _isBtnReadyToReceiveEnabled, value); }
        }

        private bool _isBtnShareEnabled = true;
        public bool IsBtnShareEnabled
        {
            get { return _isBtnShareEnabled; }
            set { SetProperty(ref _isBtnShareEnabled, value); }
        }

        private bool _isPrgUploadFileVisible = true;
        public bool IsPrgUploadFileVisible
        {
            get { return _isPrgUploadFileVisible; }
            set { SetProperty(ref _isPrgUploadFileVisible, value); }
        }

        private bool _isPrgDownloadFileVisible = true;
        public bool IsPrgDownloadFileVisible
        {
            get { return _isPrgDownloadFileVisible; }
            set { SetProperty(ref _isPrgDownloadFileVisible, value); }
        }

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
            // FIXME: Sobald der Downloadprogress vom Service funktioniert, kann die folgenden Zeile entfernt werden
            await ProgressDownloadFile.ProgressTo(0, 250, Easing.Linear);
            await Navigation.PushAsync(new QRCodeScanPage(UploadTransferFile, QRCode));
        }

        // lazy instantiation
        private ICommand _barcodeScannerCommand;
        public ICommand BarcodeScannerCommand => _barcodeScannerCommand ?? (_barcodeScannerCommand = new Command(() => BarcodeScanner()));

        async public void BarcodeScanner()
        {
            await Navigation.PushAsync(new BarcodeScannerPage(DownloadTransferFile,QRCode));
        }

        // lazy instantiation
        private ICommand _shareCommand;
        public ICommand ShareCommand => _shareCommand ?? (_shareCommand = new Command(() => Share()));

        public void Share()
        {
            // FIXME: Teilen/Share-Button kann nur geklickt werden, wenn auch ein DownloadTransferFile vorhanden ist
            ((App)Application.Current).ShareHandler.ProvideFile(DownloadTransferFile);
        }
    }
}
