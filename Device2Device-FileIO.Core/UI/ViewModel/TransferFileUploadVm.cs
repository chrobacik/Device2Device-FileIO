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
            QRCode = new QRCode();
            UploadTransferFile = uploadTransferFile;
            
            _expirationDate = DateTime.Now.AddDays(_defaultxpirationInDay);

        }

        public void TransferFileUploadVm_UploadFinished(object sender, FileOperation.UploadFinishedEventArgs e)
        {
            if (e.File.Status.State != TransferStatus.TypeState.Failed)
            {
                UploadTransferFile = e.File;



                // FIXME: Button "send" nur aktivieren, wenn ein TransferFile und Netzwerkzugriff vorhanden ist
                /*
                IsBtnSendEnabled = (UploadTransferFile != null && App.HasConnectivity()) ? true : false;
                CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
                {
                    IsBtnSendEnabled = (UploadTransferFile != null && args.IsConnected) ? true : false;
                };
                */
            }
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
