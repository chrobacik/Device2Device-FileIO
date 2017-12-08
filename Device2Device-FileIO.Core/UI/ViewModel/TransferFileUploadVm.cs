using System;
using System.IO;
using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;
using Device2DeviceFileIO.Interfaces;
using Plugin.Connectivity;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileUploadVm : BindableBase
    {
        const double _defaultxpirationInDay = 1;

        public TransferFileUploadVm(TransferFile uploadTransferFile, QRCode qRCode) {
            UploadTransferFile = uploadTransferFile;
            QRCode = qRCode;
            _expirationDate = DateTime.Now.AddDays(_defaultxpirationInDay);

            App.GetCloudFileService().UploadFinished += (object sender, FileOperation.UploadFinishedEventArgs e) => {
                // Fehlerhandling, wenn File-Status Failed
                UploadTransferFile = e.File;
                QRCode = e.Code;

                // QRCode = new QRCode();
                // QRCode.Url = e.Code.Url;

                // e.Code.CreateImage(144, 144, 0);
                // QRCode.BarCode = e.Code.BarCode;

                // QRCode.CreateImage(144, 144, 0);
            };

            // FIXME: Button "send" nur aktivieren, wenn ein TransferFile und Netzwerkzugriff vorhanden ist
            /*
            IsBtnSendEnabled = (UploadTransferFile != null && App.HasConnectivity()) ? true : false;
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                IsBtnSendEnabled = (UploadTransferFile != null && args.IsConnected) ? true : false;
            };
            */
        }

        public INavigation Navigation { get; set; }

        private bool _isBtnSendEnabled = true;
        public bool IsBtnSendEnabled
        {
            get { return _isBtnSendEnabled; }
            set { SetProperty(ref _isBtnSendEnabled, value); }
        }

        private TransferFile _uploadTransferFile;
        public TransferFile UploadTransferFile
        {
            get { return _uploadTransferFile; }
            set { SetProperty(ref _uploadTransferFile, value); }
        }

        private QRCode _qRCode;
        public QRCode QRCode
        {
            get { return _qRCode; }
            set { SetProperty(ref _qRCode, value); }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { SetProperty(ref _expirationDate, value); }
        }

        // lazy instantiation
        private ICommand _startUploadCommand;
        public ICommand StartUploadCommand => _startUploadCommand ?? (_startUploadCommand = new Command(() => StartUpload()));

        async public void StartUpload()
        {
            // FIXME: UploadTransferFile mit korrektem ExpirationDate (DatePicker aus der View) hochladen
            App.GetCloudFileService().Upload(UploadTransferFile);
            await Navigation.PopAsync();
        }
    }
}
