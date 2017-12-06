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
        private ICommand _tranferFileDownloadCmd;
        public ICommand TranferFileDownloadCommand => _tranferFileDownloadCmd ?? (_tranferFileDownloadCmd = new Command(() => TransferFileDownload()));

        async public void TransferFileDownload()
        {
            await Navigation.PushAsync(new TransferFileDownloadPage());
        }

        // lazy instantiation
        private ICommand _barcodeScannerCommand;
        public ICommand BarcodeScannerCommand => _barcodeScannerCommand ?? (_barcodeScannerCommand = new Command(() => BarcodeScanner()));

        async public void BarcodeScanner()
        {
            await Navigation.PushAsync(new BarcodeScannerPage());
        }

        // lazy instantiation
        private ICommand _scannedCmd;
        public ICommand ScannedCommand => _scannedCmd ?? (_scannedCmd = new Command(() => Scanned()));

        async public void Scanned()
        {

            DownloadTransferFile = new TransferFile();
            DownloadTransferFile.Name = "datei_bild.png";
            DownloadTransferFile.Size = 100;
            DownloadTransferFile.Status = new TransferStatus();

            await Navigation.PopAsync();
        }
    }
}
