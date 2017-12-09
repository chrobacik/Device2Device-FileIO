using System;
using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;
using Plugin.Connectivity;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileDownloadVm : BindableBase
    {
        public TransferFileDownloadVm(TransferFile downloadTransferFile, QRCode qRCode)
        {
            DownloadTransferFile = App.CurrentDownloadFile;
            QRCode = qRCode;



            // FIXME: Button "send" nur aktivieren, wenn ein TransferFile und Netzwerkzugriff vorhanden ist
            /*
            IsBtnReadyToReceiveEnabled = (DownloadTransferFile != null && App.HasConnectivity()) ? true : false;
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                IsBtnReadyToReceiveEnabled = (DownloadTransferFile != null && args.IsConnected) ? true : false;
            };
            */
        }

        public INavigation Navigation { get; set; }

        private bool _isBtnReadyToReceiveEnabled = true;
        public bool IsBtnReadyToReceiveEnabled
        {
            get { return _isBtnReadyToReceiveEnabled; }
            set { SetProperty(ref _isBtnReadyToReceiveEnabled, value); }
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
        private ICommand _startDownloadCommand;
        public ICommand StartDownloadCommand => _startDownloadCommand ?? (_startDownloadCommand = new Command(() => StartDownload()));

        async public void StartDownload()
        {
            App.GetCloudFileService().Download(QRCode);

            await Navigation.PopToRootAsync();
        }

    }
}
